﻿using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Parser
{
    using Data;

    public partial class VsnRow
    {
        public override bool IsEmpty
            => !(this.HasConversation    || this.HasDialogue      || this.HasSpeaker ||
                 this.HasActor           || this.HasActions       ||
                 this.Choice.HasValue    || this.HasGoTo          ||
                 this.HasCommandsOnStart || this.HasCommandsOnEnd ||
                 this.HasContent);

        public bool IsConversationStart
            => this.HasConversation && !this.Conversation.StartsWith('[') && !this.Conversation.EndsWith(']') &&
               !this.HasDialogue;

        public bool IsConversationEnd
            => this.HasConversation && this.Conversation.StartsWith('[') && this.Conversation.EndsWith(']') &&
               (!this.HasDialogue || this.Dialogue.Equals(EndDialogueRow.Keyword));

        public bool IsDialogue
            => !string.IsNullOrEmpty(this.Dialogue) && !this.Dialogue.Equals(EndDialogueRow.Keyword) &&
               !this.HasConversation;

        public bool IsEndDialogue
            => !string.IsNullOrEmpty(this.Dialogue) && this.Dialogue.Equals(EndDialogueRow.Keyword);

        public bool HasConversation
            => !string.IsNullOrEmpty(this.Conversation);

        public bool HasDialogue
            => !string.IsNullOrEmpty(this.Dialogue);

        public bool HasChoice
            => this.Choice.HasValue && this.Choice > 0;

        public bool HasGoTo
            => !string.IsNullOrEmpty(this.GoTo);

        public bool HasSpeaker
            => !string.IsNullOrEmpty(this.Speaker);

        public bool HasActor
            => !string.IsNullOrEmpty(this.Actor1) || !string.IsNullOrEmpty(this.Actor2) ||
               !string.IsNullOrEmpty(this.Actor3) || !string.IsNullOrEmpty(this.Actor4);

        public bool HasActions
            => !string.IsNullOrEmpty(this.Actions1) || !string.IsNullOrEmpty(this.Actions2) ||
               !string.IsNullOrEmpty(this.Actions3) || !string.IsNullOrEmpty(this.Actions4);

        public bool HasCommandsOnStart
            => !string.IsNullOrEmpty(this.CommandsOnStart);

        public bool HasCommandsOnEnd
            => !string.IsNullOrEmpty(this.CommandsOnEnd);

        public bool HasContent
        {
            get
            {
                var value = false;

                foreach (var content in this.Contents)
                {
                    if (!string.IsNullOrEmpty(content))
                    {
                        value = true;
                        break;
                    }
                }

                return value;
            }
        }

        public override string ToString()
        {
            this.stringBuilder.Clear();
            this.stringBuilder.Append($"{this.Conversation} - {this.Dialogue} - {this.Delay} - ");
            this.stringBuilder.Append($"{this.Choice} - {this.GoTo} - {this.Speaker} - ");
            this.stringBuilder.Append($"{this.Actor1} - {this.Actions1} - {this.Actor2} - {this.Actions2} - ");
            this.stringBuilder.Append($"{this.Actor3} - {this.Actions3} - {this.Actor4} - {this.Actions4} - ");
            this.stringBuilder.Append($"{this.CommandsOnStart} - {this.CommandsOnEnd}");

            foreach (var content in this.Contents)
            {
                this.stringBuilder.Append($" - {content}");
            }

            return this.stringBuilder.ToString();
        }

        public (ConversationRow, DialogueRow) Parse(INovelData data, ConversationRow conversation,
                                                    DialogueRow dialogue, in Segment<string> languages, int row,
                                                    List<string> goToList, CommandParser commandParser,
                                                    IArrayParser<int> intArrayParser, StringBuilder logger)
        {
            this.error.Clear();

            if (!this.IsEmpty)
            {
                if (this.IsConversationStart)
                    return (ParseConversationStart(data, conversation, row), dialogue);

                if (this.IsConversationEnd)
                {
                    if (this.IsEndDialogue)
                        ParseEndDialogue(conversation, commandParser, row);

                    return (ParseConversationEnd(conversation, data, goToList, logger), dialogue);
                }

                if (this.IsEndDialogue)
                    return (conversation, ParseEndDialogue(conversation, commandParser, row));

                if (this.IsDialogue)
                {
                    var newDialogue = ParseDialogue(conversation, dialogue, row, goToList, commandParser, intArrayParser, logger);

                    if (this.error.Length <= 0 && newDialogue != null)
                    {
                        ParseContent(conversation, row, languages);
                    }

                    return (conversation, newDialogue);
                }
            }

            return (conversation, dialogue);
        }

        private void ParseContent(ConversationRow conversation, int row, in Segment<string> languages)
        {
            if (conversation == null)
                return;

            conversation.AddContent(new ContentRow(row, languages, this.Contents));
        }

        private ConversationRow ParseConversationStart(INovelData data, ConversationRow conversation, int row)
        {
            var id = this.Conversation;

            if (!this.idRegex.IsMatch(id))
            {
                this.error.AppendLine($"Conversation id must only contain characters in {IdCharRange}. Current value: {id}");
                return null;
            }

            if (conversation != null && !conversation.Id.Equals(id))
            {
                this.error.AppendLine($"Conversation `{conversation.Id}` must end before starting a new one");
                return conversation;
            }

            if (data.Conversations.ContainsKey(id))
            {
                Debug.LogWarning($"Vsn row {row}: Conversation id has already existed");
            }

            return new ConversationRow(row, id);
        }

        private ConversationRow ParseConversationEnd(ConversationRow conversation, INovelData data, List<string> goToList,
                                                     StringBuilder logger)
        {
            if (conversation == null)
            {
                this.error.AppendLine("No conversation");
                return conversation;
            }

            if (string.IsNullOrEmpty(conversation.Id))
            {
                this.error.AppendLine("Invalid conversation id");
                return conversation;
            }

            var start = $"[{conversation.Id}]";
            var end = this.Conversation;

            if (!start.Equals(end))
            {
                this.error.AppendLine($"Mismatched conversation start and end tokens");
                return conversation;
            }

            VerifyGoTos(conversation, goToList, logger);
            data.AddConversation(conversation);
            return null;
        }

        private void VerifyGoTos(ConversationRow conversation, List<string> goToList, StringBuilder logger)
        {
            var dialogues = conversation.Dialogues;
            var first = true;

            for (var i = 0; i < goToList.Count; i++)
            {
                var goTo = goToList[i];

                if (string.IsNullOrEmpty(goTo))
                    continue;

                if (dialogues.ContainsKey(goTo))
                    continue;

                if (first)
                {
                    if (logger.Length > 0)
                        logger.Append("\n");

                    logger.Append($"Missing dialogues of conversation id={conversation.Id}: `{goTo}`");
                    first = false;
                    continue;
                }

                logger.Append($", `{goTo}`");
            }

            goToList.Clear();
        }

        private EndDialogueRow ParseEndDialogue(ConversationRow conversation, CommandParser commandParser, int row)
        {
            if (conversation == null)
            {
                this.error.AppendLine("No conversation");
                return null;
            }

            var commandsOnStart = commandParser.Parse(this.CommandsOnStart, this.error);

            if (this.IsError)
                return null;

            var commandsOnEnd = commandParser.Parse(this.CommandsOnEnd, this.error);

            if (this.IsError)
                return null;

            var endDialogue = new EndDialogueRow(row, this.Dialogue, commandsOnStart, commandsOnEnd);
            conversation.AddDialogue(endDialogue);

            return endDialogue;
        }

        private DialogueRow ParseDialogue(ConversationRow conversation, DialogueRow dialogue, int row,
                                          List<string> goToList, CommandParser commandParser,
                                          IArrayParser<int> intArrayParser, StringBuilder logger)
        {
            if (conversation == null)
            {
                this.error.AppendLine("No conversation");
                return null;
            }

            if (dialogue.IsEnd())
            {
                this.error.AppendLine("[END] must be the last dialogue of a conversation");
                return dialogue;
            }

            var id = this.Dialogue.Trim();

            if (dialogue != null && dialogue.Id == id)
            {
                var choice = ParseChoice(dialogue, row, goToList, logger);

                if (choice != null)
                {
                    dialogue.AddChoice(choice);
                }

                return dialogue;
            }

            if (!this.idRegex.IsMatch(id))
            {
                this.error.AppendLine($"Dialogue id must only contain characters in {IdCharRange}. Current value: {this.Dialogue}");
                return null;
            }

            if (conversation.Dialogues.ContainsKey(id))
            {
                Debug.LogWarning($"Vsn row {row}: Dialogue id has already existed");
            }

            var actions1 = commandParser.Parse(this.Actions1, this.error);

            if (this.IsError)
                return null;

            var actions2 = commandParser.Parse(this.Actions2, this.error);

            if (this.IsError)
                return null;

            var actions3 = commandParser.Parse(this.Actions3, this.error);

            if (this.IsError)
                return null;

            var actions4 = commandParser.Parse(this.Actions4, this.error);

            if (this.IsError)
                return null;

            var highlight = intArrayParser.Parse(this.Highlight, this.error);

            if (this.IsError)
                return null;

            var commandsOnStart = commandParser.Parse(this.CommandsOnStart, this.error);

            if (this.IsError)
                return null;

            var commandsOnEnd = commandParser.Parse(this.CommandsOnEnd, this.error);

            if (this.IsError)
                return null;

            var newDialogue = new DialogueRow(row, id, this.Delay ?? 0f,
                                              this.Speaker?.Trim() ?? string.Empty,
                                              this.Actor1?.Trim() ?? string.Empty, actions1,
                                              this.Actor2?.Trim() ?? string.Empty, actions2,
                                              this.Actor3?.Trim() ?? string.Empty, actions3,
                                              this.Actor4?.Trim() ?? string.Empty, actions4,
                                              highlight, commandsOnStart, commandsOnEnd);

            var newChoice = ParseChoice(newDialogue, row, goToList, logger);

            if (newChoice != null)
            {
                newDialogue.AddChoice(newChoice);
                conversation.AddDialogue(newDialogue);

                return newDialogue;
            }

            return null;
        }

        private ChoiceRow ParseChoice(DialogueRow dialogue, int row, List<string> goToList, StringBuilder logger)
        {
            var id = this.HasChoice ? this.Choice.Value : 0;

            if (!dialogue.GetChoice(id).IsNullOrNone())
            {
                var str = id == 0 ? "default choice" : $"choice with id={id}";
                this.error.AppendLine($"Dialogue `{dialogue.Id}` has already contained a {str}");
                return null;
            }

            var goTo = this.GoTo?.Trim() ?? string.Empty;

            if (goTo.Length > 0 && !goTo.Equals(EndDialogueRow.Keyword) &&
                !this.idRegex.IsMatch(goTo))
            {
                this.error.AppendLine($"Go_To must be either empty or a valid dialogue id. Current value: {this.GoTo}");
                return null;
            }

            if (string.IsNullOrEmpty(goTo) && id != 0)
            {
                if (logger.Length > 0)
                    logger.Append("\n");

                logger.Append($"Vns row {row}: Go_To cannot be null or empty");
            }

            goToList.Add(goTo);
            return new ChoiceRow(row, id, goTo);
        }
    }
}
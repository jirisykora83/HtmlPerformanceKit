﻿using HtmlSpeedPack.Infrastructure;

namespace HtmlSpeedPack.StateMachine
{
    internal partial class HtmlStateMachine
    {
        /// <summary>
        /// 8.2.4.29 Script data double escaped state
        ///
        /// Consume the next input character:
        /// 
        /// "-" (U+002D)
        /// Switch to the script data double escaped dash state. Emit a U+002D HYPHEN-MINUS character token.
        /// 
        /// "&lt;" (U+003C)
        /// Switch to the script data double escaped less-than sign state. Emit a U+003C LESS-THAN SIGN character token.
        /// 
        /// U+0000 NULL
        /// Parse error. Emit a U+FFFD REPLACEMENT CHARACTER character token.
        /// 
        /// EOF
        /// Parse error. Switch to the data state. Reconsume the EOF character.
        /// 
        /// Anything else
        /// Emit the current input character as a character token.
        /// </summary>
        private void ScriptDataDoubleEscapedState()
        {
            var currentInputCharacter = bufferReader.Consume();

            switch (currentInputCharacter)
            {
                case '-':
                    State = ScriptDataDoubleEscapedDashState;
                    currentDataBuffer.Append('-');
                    return;

                case '<':
                    State = ScriptDataDoubleEscapedLessThanSignState;
                    currentDataBuffer.Append('<');
                    return;

                case HtmlChar.Null:
                    ParseError = ParseErrorMessage.UnexpectedNullCharacterInStream;
                    currentDataBuffer.Append(HtmlChar.ReplacementCharacter);
                    return;

                case EofMarker:
                    ParseError = ParseErrorMessage.UnexpectedEndOfFile;
                    State = DataState;
                    bufferReader.Reconsume(EofMarker);
                    return;
                    
                default:
                    currentDataBuffer.Append((char)currentInputCharacter);
                    return;
            }
        }
    }
}

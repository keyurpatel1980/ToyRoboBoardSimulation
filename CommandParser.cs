using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyRobotSimulation
{
    public interface ICommandParser
    {
        Command ParseCommand(string cmdText, int maxX, int maxY);
    }
    public class CommandParser : ICommandParser
    {
        public Command ParseCommand(string cmdText, int maxX, int maxY)
        {
            Command command = new Command { CommandText = cmdText };
            if (string.IsNullOrWhiteSpace(cmdText))
                return command;

            var commandTokens = GetCommandTextTokens(cmdText);

            if (!commandTokens.Any())
                return command;

            command = GetCommandFromTokens(commandTokens, maxX, maxY, command);

            return command;
        }
        private List<CommandToken> GetCommandTextTokens(string cmdText)
        {
            List<CommandToken> commandTokens = new List<CommandToken>();
            // Firsttoken is Verb, Second must be x coordinate, Third must be y coordinate, Fourth must be direction
            var tokens = cmdText.Split(' '); // e.g. PLACE 0,0,NORTH
            if (!tokens.Any())
                return commandTokens;

            int tokenIndex = 0;
            foreach (var token in tokens)
            {
                if (tokenIndex == 0)
                {
                    CommandToken ct = new CommandToken { TokenIndex = tokenIndex, Token = token };
                    commandTokens.Add(ct);
                    tokenIndex++;
                    continue;
                }

                if (!token.Contains(","))
                    continue;

                GetSecondCommandTextTokens(token, commandTokens);

                tokenIndex++;
            }
            return commandTokens;
        }
        private void GetSecondCommandTextTokens(string token, List<CommandToken> commandTokens)
        {
            // e.g. 0,0,NORTH
            var subTokens = token.Split(',');
            int sIndex = 1; // start from 1 instead of 0 because it could be a second token out of 4
            foreach (var st in subTokens)
            {
                if (sIndex == 1)
                {
                    CommandToken ct = new CommandToken { TokenIndex = sIndex, Token = st };
                    commandTokens.Add(ct);
                }

                else if (sIndex == 2)
                {
                    CommandToken ct = new CommandToken { TokenIndex = sIndex, Token = st };
                    commandTokens.Add(ct);
                }

                else if (sIndex == 3 && !string.IsNullOrEmpty(st))
                {
                    CommandToken ct = new CommandToken { TokenIndex = sIndex, Token = st };
                    commandTokens.Add(ct);
                }
                sIndex++;
            }
        }
        private Command GetCommandFromTokens(List<CommandToken> commandTokens, int maxX, int maxY, Command command)
        {
            // e.g. = Tokens = {PLACE, 0, 0, NORTH} => command => PLACE 0,0,NORTH 
            foreach (var token in commandTokens.ToArray())
            {
                if (token.TokenIndex.HasValue && token.TokenIndex == 0 && !string.IsNullOrEmpty(token.Token))
                    ValidateFirstToken(token, command);
                if (token.TokenIndex.HasValue && token.TokenIndex == 1 && !string.IsNullOrEmpty(token.Token))
                    ValidateSecondToken(token, command, maxX);
                if (token.TokenIndex.HasValue && token.TokenIndex == 2 && !string.IsNullOrEmpty(token.Token))
                    ValidateThirdToken(token, command, maxY);
                if (token.TokenIndex.HasValue && token.TokenIndex == 3 && !string.IsNullOrEmpty(token.Token))
                    ValidateFourthToken(token, command);
            }

            return command;
        }
        private void ValidateFirstToken(CommandToken token, Command command)
        {
            // it is already checked that first token exists
            if (EnumHelpers<CommandVerb>.IsPresent(token.Token))// Must be a valid verb
                command.CommandVerb = (CommandVerb)Enum.Parse(typeof(CommandVerb), token.Token);
            else
            {
                command.IsFirstTokenInvalid = true;
                command.FirstTokenError = string.Format($"Invalid Verb: {token.Token}");
            }
        }
        private void ValidateSecondToken(CommandToken token, Command command, int maxX)
        {
            // it is already checked that second token exists
            if (int.TryParse(token.Token, out int x) && x >= 0 && x <= maxX) // must be within bounds of the board
                command.XCord = x;
            else
            {
                command.IsSecondTokenInvalid = true;
                command.SecondTokenError = string.Format($"Invalid X Coordinate: {token.Token} (valid (0,{maxX})");
            }
        }
        private void ValidateThirdToken(CommandToken token, Command command, int maxY)
        {
            // it is already checked that third token exists
            if (int.TryParse(token.Token, out int y) && y >= 0 && y <= maxY) // must be within bounds of the board
                command.YCord = y;
            else
            {
                command.IsThirdTokenInvalid = true;
                command.ThirdTokenError = string.Format($"Invalid Y Coordinate: {token.Token} (valid (0,{maxY}))");
            }
        }
        private void ValidateFourthToken(CommandToken token, Command command)
        {
            // it is already checked that fourth token exists
            if (EnumHelpers<Direction>.IsPresent(token.Token)) // must be valid direction
                command.Direction = (Direction)Enum.Parse(typeof(Direction), token.Token);
            else
            {
                command.IsFourthTokenInvalid = true;
                command.FourthTokenError = string.Format($"Invalid Direction: {token.Token}");
            }
        }
    }
}

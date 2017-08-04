using System.Linq;

namespace Battleship.Domain.ReadModel
{
    public class GameDetails : ReadModelBase
    {
        public object ActivatedOn { get; set; }
        public Player[] Players { get; } = {new Player(), new Player()};
        public Player HasWinner => Players.Any(p => !p.HasActiveShips) ? Players.First(p => p.HasActiveShips) : null;
        public uint Turn { get; set; }
        public uint Dimensions { get; set; }

        public bool IsValidLocation(Location location, uint playerIndex)
        {
            return Players[playerIndex].Board.ValidLocations.Contains(location.GetHashCode());
        }

        public bool IsValidShipLocation(ShipDetails ship, uint playerIndex)
        {
            return Players[playerIndex].Board.ShipFitsOnBoard(ship);
        }

        #region Basic Input Validation

        // The game generally knows that a particular value is a valid selection given the dimensions
        // Only boards know about specific rules for ship placement
        public bool ValidRowSelection(char row)
        {
            return GameConsts.Alphabet.Substring(0, (int) Dimensions).Contains(char.ToUpper(row).ToString());
        }

        public bool ValidColumnSelection(uint col)
        {
            return col > 0 && col <= Dimensions;
        }

        #endregion
    }
}
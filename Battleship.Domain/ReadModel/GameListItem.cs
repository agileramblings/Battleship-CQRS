using System.Collections.Generic;

namespace Battleship.Domain.ReadModel
{
    public class GameListItem : ReadModelBase
    {
        public string GameName { get; set; }
        public string GameCode { get; set; }
        public List<string> Players { get; set; }
    }
}
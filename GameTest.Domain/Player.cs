using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest.Domain
{
    public class Player
    {
        public Player(string connectionId, string name)
        {
            ConnectionId = connectionId;
            Name = name;
        }

        public string ConnectionId { get; }
        public string Name { get; set; }
        public int Score { get; set; }
    }
}

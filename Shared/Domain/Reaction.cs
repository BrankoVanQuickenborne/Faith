using Faith.Shared.DTOs;
using System;

namespace Faith.Shared.Domain
{
    public class Reaction
    {
        public Guid ReactionID { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public Reaction() { }
        public Reaction(ReactionDTO reaction)
        {
            ReactionID = reaction.ReactionID;
            Text = reaction.Text;
            Date = reaction.Date;
        }
    }
}

using Faith.Shared.Domain;
using System;

namespace Faith.Shared.DTOs
{
    public class ReactionDTO
    {
        public Guid ReactionID { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public ReactionDTO() { }
        public ReactionDTO(Reaction reaction)
        {
            ReactionID = reaction.ReactionID;
            Text = reaction.Text;
            Date = reaction.Date;
        }
    }
}

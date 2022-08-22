using Faith.Shared.Domain;
using System;
using System.Collections.Generic;

namespace Faith.Shared.DTOs
{
    public class PostDTO
    {
        public Guid PostID { get; set; }
        public string Text { get; set; }
        public byte[] Picture { get; set; }
        public bool Archived { get; set; }
        public DateTime Date { get; set; }
        public ICollection<ReactionDTO> Reactions { get; set; }

        public PostDTO()
        {
            Reactions = new List<ReactionDTO>();
        }
        public PostDTO(Post post)
        {
            PostID = post.PostID;
            Text = post.Text;
            Picture = post.Picture;
            Archived = post.Archived;
            Date = post.Date;
            if (post.Reactions != null)
            {
                Reactions = new List<ReactionDTO>();
                foreach (var reaction in post.Reactions)
                {
                    Reactions.Add(new ReactionDTO(reaction));
                }
            }
        }
    }
}

using Faith.Shared.DTOs;
using System;
using System.Collections.Generic;

namespace Faith.Shared.Domain
{
    public class Post
    {
        public Guid PostID { get; set; }
        public string Text { get; set; }
        public byte[] Picture { get; set; }
        public bool Archived { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Reaction> Reactions { get; set; }

        public Post() 
        {
            PostID = new Guid();
            Reactions = new List<Reaction>();
        }
        public Post(PostDTO post)
        {
            PostID = post.PostID;
            Text = post.Text;
            Picture = post.Picture;
            Archived = post.Archived;
            Date = post.Date;
            if (post.Reactions != null)
            {
                Reactions = new List<Reaction>();
                foreach (var reaction in post.Reactions)
                {
                    Reactions.Add(new Reaction(reaction));
                }
            }
        }
    }
}

using Faith.Shared.Domain;
using System;
using System.Collections.Generic;

namespace Faith.Shared.DTOs
{
    public class TimeLineDTO
    {
        public Guid TimeLineID { get; set; } = new Guid();
        public ICollection<PostDTO> Posts { get; set; }

        public TimeLineDTO()
        {
            Posts = new List<PostDTO>();
        }

        public TimeLineDTO(TimeLine timeline)
        {
            TimeLineID = timeline.TimeLineID;
            if (timeline.Posts != null)
            {
                Posts = new List<PostDTO>();
                foreach (var post in timeline.Posts)
                {
                    Posts.Add(new PostDTO(post));
                }
            }
        }
    }
}

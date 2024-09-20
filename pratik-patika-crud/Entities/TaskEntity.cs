﻿namespace pratik_patika_crud.Entities
{
    public class TaskEntity
    {

        public TaskEntity()
        {
            CreatedDate = DateTime.Now;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public bool IsDone { get; set; }
        public bool IsDelete { get; set; }
        public int OwnerId { get; set; }
        
    }
}

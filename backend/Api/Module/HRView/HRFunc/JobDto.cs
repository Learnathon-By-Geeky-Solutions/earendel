using System;

namespace TalentMesh.Module.HRView.HRFunc
{
    public class JobDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string Location { get; set; }
        public string JobType { get; set; }
        public string ExperienceLevel { get; set; }
        public string Salary { get; set; }
        public string PaymentStatus { get; set; }
        public Guid PostedById { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset LastModifiedOn { get; set; }
    }
}
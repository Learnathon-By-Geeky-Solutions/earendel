using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentMesh.Module.Candidate.Domain;

namespace TalentMesh.Module.Candidate.Infrastructure.Persistence.Configurations
{
    public class CandidateProfileConfigurations : IEntityTypeConfiguration<CandidateProfile>
    {
        public void Configure(EntityTypeBuilder<CandidateProfile> builder)
        {
            // Specify the table name.
            builder.ToTable("CandidateProfiles");

            // Define the primary key.
            builder.HasKey(cp => cp.Id);

            // Configure properties with example max length and nullability.
            builder.Property(cp => cp.Resume)
                   .HasMaxLength(2000)
                   .IsRequired(false);

            builder.Property(cp => cp.Skills)
                   .HasMaxLength(1000)
                   .IsRequired(false);

            builder.Property(cp => cp.Experience)
                   .HasMaxLength(2000)
                   .IsRequired(false);

            builder.Property(cp => cp.Education)
                   .HasMaxLength(1000)
                   .IsRequired(false);

            // The UserId is required as it links the candidate profile to the user.
            builder.Property(cp => cp.UserId)
                   .IsRequired();
        }
    }
}

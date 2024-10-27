using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SiyaphambiliTutorials.Data;

namespace SiyaphambiliTutorials.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        
    }



    // User Management
    public DbSet<Student> Students { get; set; }
    public DbSet<Tutor> Tutors { get; set; }
    public DbSet<Administrator> Administrators { get; set; }

    // Course and Enrollment
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<CourseModule> CourseModules { get; set; }
    public DbSet<ModuleContent> ModuleContents { get; set; }

    // Tutoring Sessions
    public DbSet<TutoringSession> TutoringSessions { get; set; }
    public DbSet<SessionBooking> SessionBookings { get; set; }

    // Study Materials
    public DbSet<StudyMaterial> StudyMaterials { get; set; }
    public DbSet<MaterialPurchase> MaterialPurchases { get; set; }

    // Quizzes and Assessments
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<QuizQuestion> QuizQuestions { get; set; }
    public DbSet<QuizOption> QuizOptions { get; set; }
    public DbSet<QuizAttempt> QuizAttempts { get; set; }
    public DbSet<QuizAnswer> QuizAnswers { get; set; }

    // Library Management
    public DbSet<LibraryResource> LibraryResources { get; set; }
    public DbSet<ResourceLoan> ResourceLoans { get; set; }

    // Payment and Billing
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    // Notifications and Messaging
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Message> Messages { get; set; }

    // Gamification
    public DbSet<Badge> Badges { get; set; }
    public DbSet<UserBadge> UserBadges { get; set; }

    // Community Features
    public DbSet<ForumTopic> ForumTopics { get; set; }
    public DbSet<ForumThread> ForumThreads { get; set; }
    public DbSet<ForumPost> ForumPosts { get; set; }

    // Administrative
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<StudentModuleContentCompletion> StudentModuleContentCompletions {get;set;}




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure one-to-one relationships
        modelBuilder.Entity<Student>()
            .HasOne(s => s.User)
            .WithOne(u => u.StudentProfile)
            .HasForeignKey<Student>(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Tutor>()
            .HasOne(t => t.User)
            .WithOne(u => u.TutorProfile)
            .HasForeignKey<Tutor>(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Administrator>()
            .HasOne(a => a.User)
            .WithOne(u => u.AdminProfile)
            .HasForeignKey<Administrator>(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the Message entity relationships
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Recipient)
            .WithMany()
            .HasForeignKey(m => m.RecipientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the one-to-many relationship between ForumTopic and ForumThread
        modelBuilder.Entity<ForumThread>()
            .HasOne(ft => ft.ForumTopic)
            .WithMany(ftopic => ftopic.ForumThreads)
            .HasForeignKey(ft => ft.ForumTopicId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

        // Configure the relationship between ForumThread and User (CreatedByUser)
        modelBuilder.Entity<ForumThread>()
            .HasOne(ft => ft.CreatedByUser)
            .WithMany()
            .HasForeignKey(ft => ft.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

        // Configure the relationship between QuizAnswer and QuizAttempt
        modelBuilder.Entity<QuizAnswer>()
            .HasOne(qa => qa.QuizAttempt)
            .WithMany(qa => qa.Answers)
            .HasForeignKey(qa => qa.QuizAttemptId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

        // Configure the relationship between QuizAnswer and QuizQuestion
        modelBuilder.Entity<QuizAnswer>()
            .HasOne(qa => qa.QuizQuestion)
            .WithMany(q => q.QuizeAnswers)
            .HasForeignKey(qa => qa.QuizQuestionId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

        // Additional configurations can be added here

    }

}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ThiTracNghiem_BackEndAPI.Models
{
    public partial class tracnghiemContext : DbContext
    {
        public tracnghiemContext()
        {
        }

        public tracnghiemContext(DbContextOptions<tracnghiemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answers> Answers { get; set; }
        public virtual DbSet<Cache> Cache { get; set; }
        public virtual DbSet<Exams> Exams { get; set; }
        public virtual DbSet<Examtypes> Examtypes { get; set; }
        public virtual DbSet<Joinroom> Joinroom { get; set; }
        public virtual DbSet<Migrations> Migrations { get; set; }
        public virtual DbSet<Modules> Modules { get; set; }
        public virtual DbSet<OauthAccessTokens> OauthAccessTokens { get; set; }
        public virtual DbSet<OauthAuthCodes> OauthAuthCodes { get; set; }
        public virtual DbSet<OauthClients> OauthClients { get; set; }
        public virtual DbSet<OauthPersonalAccessClients> OauthPersonalAccessClients { get; set; }
        public virtual DbSet<OauthRefreshTokens> OauthRefreshTokens { get; set; }
        public virtual DbSet<Postcategories> Postcategories { get; set; }
        public virtual DbSet<Posts> Posts { get; set; }
        public virtual DbSet<Questiondetails> Questiondetails { get; set; }
        public virtual DbSet<Questions> Questions { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Rooms> Rooms { get; set; }
        public virtual DbSet<Scores> Scores { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answers>(entity =>
            {
                entity.ToTable("answers");

                entity.HasIndex(e => e.QuestionId)
                    .HasName("QuestionID");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.A)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AnswerExplain)
                    .HasColumnName("answerExplain")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'''Chưa có'''");

                entity.Property(e => e.B)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.C)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CorrectAnswers)
                    .IsRequired()
                    .HasColumnName("correctAnswers")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.D)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.QuestionId)
                    .HasColumnName("question_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("answers_ibfk_1");
            });

            modelBuilder.Entity<Cache>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("cache");

                entity.HasIndex(e => e.Key)
                    .HasName("cache_key_unique")
                    .IsUnique();

                entity.Property(e => e.Expiration)
                    .HasColumnName("expiration")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasColumnName("key")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasColumnType("mediumtext");
            });

            modelBuilder.Entity<Exams>(entity =>
            {
                entity.ToTable("exams");

                entity.HasIndex(e => e.ExamSlug)
                    .HasName("TitleSlug")
                    .IsUnique();

                entity.HasIndex(e => e.ExamTitle)
                    .HasName("ExamTitle")
                    .IsUnique();

                entity.HasIndex(e => e.ExamtypeId)
                    .HasName("ExamType");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("dateCreated")
                    .HasDefaultValueSql("'current_timestamp()'");

                entity.Property(e => e.ExamDescription)
                    .HasColumnName("examDescription")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.ExamSlug)
                    .HasColumnName("examSlug")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.ExamTitle)
                    .IsRequired()
                    .HasColumnName("examTitle")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExamtypeId)
                    .HasColumnName("examtype_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.TimeLimit)
                    .HasColumnName("timeLimit")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.TotalQuestions)
                    .HasColumnName("totalQuestions")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Examtype)
                    .WithMany(p => p.Exams)
                    .HasForeignKey(d => d.ExamtypeId)
                    .HasConstraintName("exams_ibfk_1");
            });

            modelBuilder.Entity<Examtypes>(entity =>
            {
                entity.ToTable("examtypes");

                entity.HasIndex(e => e.TypeTitle)
                    .HasName("TypeTitle_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TypeDescription)
                    .HasColumnName("typeDescription")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.TypeSlug)
                    .IsRequired()
                    .HasColumnName("typeSlug")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TypeTitle)
                    .IsRequired()
                    .HasColumnName("typeTitle")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Joinroom>(entity =>
            {
                entity.ToTable("joinroom");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RoomId)
                    .HasColumnName("room_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Score)
                    .HasColumnName("score")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.TimeSubmitExam)
                    .HasColumnName("time_submit_exam")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Migrations>(entity =>
            {
                entity.ToTable("migrations");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Batch)
                    .HasColumnName("batch")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Migration)
                    .IsRequired()
                    .HasColumnName("migration")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Modules>(entity =>
            {
                entity.ToTable("modules");

                entity.HasIndex(e => e.Id)
                    .HasName("moduleID");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ModuleDescription)
                    .HasColumnName("moduleDescription")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.ModuleName)
                    .IsRequired()
                    .HasColumnName("moduleName")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OauthAccessTokens>(entity =>
            {
                entity.ToTable("oauth_access_tokens");

                entity.HasIndex(e => e.UserId)
                    .HasName("oauth_access_tokens_user_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ClientId)
                    .HasColumnName("client_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.ExpiresAt)
                    .HasColumnName("expires_at")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Revoked)
                    .HasColumnName("revoked")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Scopes)
                    .HasColumnName("scopes")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("bigint(20)")
                    .HasDefaultValueSql("'NULL'");
            });

            modelBuilder.Entity<OauthAuthCodes>(entity =>
            {
                entity.ToTable("oauth_auth_codes");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ClientId)
                    .HasColumnName("client_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.ExpiresAt)
                    .HasColumnName("expires_at")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Revoked)
                    .HasColumnName("revoked")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Scopes)
                    .HasColumnName("scopes")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("bigint(20)");
            });

            modelBuilder.Entity<OauthClients>(entity =>
            {
                entity.ToTable("oauth_clients");

                entity.HasIndex(e => e.UserId)
                    .HasName("oauth_clients_user_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordClient)
                    .HasColumnName("password_client")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PersonalAccessClient)
                    .HasColumnName("personal_access_client")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Redirect)
                    .IsRequired()
                    .HasColumnName("redirect");

                entity.Property(e => e.Revoked)
                    .HasColumnName("revoked")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Secret)
                    .IsRequired()
                    .HasColumnName("secret")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("bigint(20)")
                    .HasDefaultValueSql("'NULL'");
            });

            modelBuilder.Entity<OauthPersonalAccessClients>(entity =>
            {
                entity.ToTable("oauth_personal_access_clients");

                entity.HasIndex(e => e.ClientId)
                    .HasName("oauth_personal_access_clients_client_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.ClientId)
                    .HasColumnName("client_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("'NULL'");
            });

            modelBuilder.Entity<OauthRefreshTokens>(entity =>
            {
                entity.ToTable("oauth_refresh_tokens");

                entity.HasIndex(e => e.AccessTokenId)
                    .HasName("oauth_refresh_tokens_access_token_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AccessTokenId)
                    .IsRequired()
                    .HasColumnName("access_token_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ExpiresAt)
                    .HasColumnName("expires_at")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Revoked)
                    .HasColumnName("revoked")
                    .HasColumnType("tinyint(1)");
            });

            modelBuilder.Entity<Postcategories>(entity =>
            {
                entity.ToTable("postcategories");

                entity.HasIndex(e => e.CategoryName)
                    .HasName("CategoryName")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CategoryDescription)
                    .IsRequired()
                    .HasColumnName("categoryDescription")
                    .HasMaxLength(600)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnName("categoryName")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryType)
                    .HasColumnName("categoryType")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'2'")
                    .HasComment("1 - Kiến thức module, 2 - Khác");
            });

            modelBuilder.Entity<Posts>(entity =>
            {
                entity.ToTable("posts");

                entity.HasIndex(e => e.PostSlug)
                    .HasName("PostSlug")
                    .IsUnique();

                entity.HasIndex(e => e.PostTitle)
                    .HasName("PostTitle_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.PostcategoryId)
                    .HasName("ModuleID");

                entity.HasIndex(e => e.UserId)
                    .HasName("AccountID");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PostContent)
                    .IsRequired()
                    .HasColumnName("postContent");

                entity.Property(e => e.PostDate)
                    .HasColumnName("postDate")
                    .HasDefaultValueSql("'current_timestamp()'");

                entity.Property(e => e.PostSlug)
                    .IsRequired()
                    .HasColumnName("postSlug")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PostTitle)
                    .IsRequired()
                    .HasColumnName("postTitle")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PostcategoryId)
                    .HasColumnName("postcategory_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Thumbnail)
                    .HasColumnName("thumbnail")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Views)
                    .HasColumnName("views")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Postcategory)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.PostcategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("posts_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("posts_ibfk_2");
            });

            modelBuilder.Entity<Questiondetails>(entity =>
            {
                entity.HasKey(e => new { e.QuestionId, e.ExamId })
                    .HasName("PRIMARY");

                entity.ToTable("questiondetails");

                entity.HasIndex(e => e.ExamId)
                    .HasName("ExamID");

                entity.Property(e => e.QuestionId)
                    .HasColumnName("question_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ExamId)
                    .HasColumnName("exam_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.Questiondetails)
                    .HasForeignKey(d => d.ExamId)
                    .HasConstraintName("questiondetails_ibfk_2");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Questiondetails)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("questiondetails_ibfk_1");
            });

            modelBuilder.Entity<Questions>(entity =>
            {
                entity.ToTable("questions");

                entity.HasIndex(e => e.ModuleId)
                    .HasName("ModuleID");

                entity.HasIndex(e => e.QuestionType)
                    .HasName("QuestionType");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("dateCreated")
                    .HasDefaultValueSql("'current_timestamp()'");

                entity.Property(e => e.ModuleId)
                    .HasColumnName("module_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.QuestionContent)
                    .IsRequired()
                    .HasColumnName("questionContent")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.QuestionType)
                    .HasColumnName("questionType")
                    .HasColumnType("int(11)")
                    .HasComment("1-Câu hỏi có một câu trả lời, 2-Câu hỏi có nhiều câu trả lời");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("questions_ibfk_1");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.ToTable("roles");

                entity.HasIndex(e => e.RoleTitle)
                    .HasName("LevelTitle_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RoleDescription)
                    .HasColumnName("roleDescription")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.RoleTitle)
                    .IsRequired()
                    .HasColumnName("roleTitle")
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Rooms>(entity =>
            {
                entity.ToTable("rooms");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("'current_timestamp()'");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.ExamId)
                    .HasColumnName("exam_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PublicRoom)
                    .HasColumnName("public_room")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.RoomCode)
                    .IsRequired()
                    .HasColumnName("roomCode")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RoomName)
                    .HasColumnName("roomName")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("'current_timestamp()'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Scores>(entity =>
            {
                entity.ToTable("scores");

                entity.HasIndex(e => e.ExamId)
                    .HasName("ExamID");

                entity.HasIndex(e => e.UserId)
                    .HasName("AccountID");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasDefaultValueSql("'current_timestamp()'");

                entity.Property(e => e.ExamId)
                    .HasColumnName("exam_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.Scores)
                    .HasForeignKey(d => d.ExamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("scores_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Scores)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("scores_ibfk_1");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.RoleId)
                    .HasName("role_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("firstName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'3'")
                    .HasComment("1-Nam, 2-Nữ, 3-Không xác định");

                entity.Property(e => e.JoinDate)
                    .HasColumnName("joinDate")
                    .HasDefaultValueSql("'current_timestamp()'");

                entity.Property(e => e.LastName)
                    .HasColumnName("lastName")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasComment("Default: 123456");

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phoneNumber")
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.WorkPlace)
                    .HasColumnName("workPlace")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'NULL'");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("users_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

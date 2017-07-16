using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CmsEngine.Data;
using CmsEngine;

namespace CmsEngine.Migrations
{
    [DbContext(typeof(CmsEngineContext))]
    partial class CmsEngineContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CmsEngine.Data.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<string>("UserCreated")
                        .HasMaxLength(20);

                    b.Property<string>("UserModified")
                        .HasMaxLength(20);

                    b.Property<Guid>("VanityId")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("newid()");

                    b.Property<int>("WebsiteId");

                    b.HasKey("Id");

                    b.HasIndex("WebsiteId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("CmsEngine.Data.Models.Page", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("DocumentContent")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("PublishedOn");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<int>("Status");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("UserCreated")
                        .HasMaxLength(20);

                    b.Property<string>("UserModified")
                        .HasMaxLength(20);

                    b.Property<Guid>("VanityId")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("newid()");

                    b.Property<int>("WebsiteId");

                    b.HasKey("Id");

                    b.HasIndex("WebsiteId");

                    b.ToTable("Pages");
                });

            modelBuilder.Entity("CmsEngine.Data.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("DocumentContent")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("PublishedOn");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<int>("Status");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("UserCreated")
                        .HasMaxLength(20);

                    b.Property<string>("UserModified")
                        .HasMaxLength(20);

                    b.Property<Guid>("VanityId")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("newid()");

                    b.Property<int>("WebsiteId");

                    b.HasKey("Id");

                    b.HasIndex("WebsiteId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("CmsEngine.Data.Models.PostCategory", b =>
                {
                    b.Property<int>("PostId");

                    b.Property<int>("CategoryId");

                    b.HasKey("PostId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("PostCategory");
                });

            modelBuilder.Entity("CmsEngine.Data.Models.PostTag", b =>
                {
                    b.Property<int>("PostId");

                    b.Property<int>("TagId");

                    b.HasKey("PostId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("PostTag");
                });

            modelBuilder.Entity("CmsEngine.Data.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<string>("UserCreated")
                        .HasMaxLength(20);

                    b.Property<string>("UserModified")
                        .HasMaxLength(20);

                    b.Property<Guid>("VanityId")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("newid()");

                    b.Property<int>("WebsiteId");

                    b.HasKey("Id");

                    b.HasIndex("WebsiteId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("CmsEngine.Data.Models.Website", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Culture")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("DateFormat")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("SiteUrl")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("UrlFormat")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("UserCreated")
                        .HasMaxLength(20);

                    b.Property<string>("UserModified")
                        .HasMaxLength(20);

                    b.Property<Guid>("VanityId")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("newid()");

                    b.HasKey("Id");

                    b.ToTable("Websites");
                });

            modelBuilder.Entity("CmsEngine.Data.Models.Category", b =>
                {
                    b.HasOne("CmsEngine.Data.Models.Website", "Website")
                        .WithMany("Categories")
                        .HasForeignKey("WebsiteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CmsEngine.Data.Models.Page", b =>
                {
                    b.HasOne("CmsEngine.Data.Models.Website", "Website")
                        .WithMany("Pages")
                        .HasForeignKey("WebsiteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CmsEngine.Data.Models.Post", b =>
                {
                    b.HasOne("CmsEngine.Data.Models.Website", "Website")
                        .WithMany("Posts")
                        .HasForeignKey("WebsiteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CmsEngine.Data.Models.PostCategory", b =>
                {
                    b.HasOne("CmsEngine.Data.Models.Category", "Category")
                        .WithMany("PostCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CmsEngine.Data.Models.Post", "Post")
                        .WithMany("PostCategories")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CmsEngine.Data.Models.PostTag", b =>
                {
                    b.HasOne("CmsEngine.Data.Models.Post", "Post")
                        .WithMany("PostTags")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CmsEngine.Data.Models.Tag", "Tag")
                        .WithMany("PostTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CmsEngine.Data.Models.Tag", b =>
                {
                    b.HasOne("CmsEngine.Data.Models.Website", "Website")
                        .WithMany("Tags")
                        .HasForeignKey("WebsiteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}

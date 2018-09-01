using System;
using CmsEngine.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CmsEngine.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder builder)
        {
            var website = new Website
            {
                Id = 1,
                VanityId = Guid.NewGuid(),
                Name = "Sample Website",
                Description = "This is a sample website",
                Culture = "en-US",
                UrlFormat = "http://[site_url]/[type]/[slug]",
                DateFormat = "MM/dd/yyyy",
                SiteUrl = "cmsengine.test",
                ArticleLimit = 10
            };

            var applicationUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "john@doe.com",
                Email = "john@doe.com",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                NormalizedEmail = "JOHN@DOE.COM",
                NormalizedUserName = "JOHN@DOE.COM",
                PasswordHash = "AQAAAAEAACcQAAAAEGIUaLe7RWZGw8Tr5/xoUMOooAzJsLFw550fDqZkrbk8CD+urHQzYjK1xY8vcDMekw==", // P@ssword1
                SecurityStamp = "NBTDBYKTNLGHKQ3HI7YFEHPQN5YRXWQC",
                TwoFactorEnabled = false,
                Name = "John",
                Surname = "Doe"
            };

            string documentContent = @"<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed risus libero, egestas vel tempus id, venenatis nec tellus. Nullam hendrerit id magna quis venenatis. Pellentesque rhoncus leo vitae turpis tristique, nec placerat tellus scelerisque. Aenean vitae rhoncus urna, non posuere elit. Nullam quam libero, porttitor in lectus convallis, pellentesque finibus libero. Suspendisse potenti. Fusce quis purus egestas, malesuada massa sed, dignissim purus. Curabitur vitae rhoncus nulla, sit amet dignissim quam.</p>
                                       <p>Mauris lorem urna, convallis in enim nec, tristique ullamcorper nisl. Fusce nec tellus et arcu imperdiet ullamcorper vestibulum vitae mi. Sed bibendum molestie dolor sit amet rhoncus.Duis consectetur convallis auctor. In hac habitasse platea dictumst.Duis lorem nibh, mattis ut purus interdum, scelerisque molestie est. Nullam molestie a est vel ornare. Maecenas rhoncus accumsan ligula, at pretium purus tempus ut. Aliquam erat nulla, pretium vel eros vitae, blandit aliquam nibh. Nulla tincidunt, justo et ullamcorper dictum, augue lectus dictum ligula, eget rutrum sem nibh non felis.Aenean elementum, sem sit amet pulvinar tempus, neque eros faucibus turpis, quis molestie nisi libero quis purus.</p>
                                       <p>Donec quam massa, tincidunt eu lacus in, lacinia hendrerit urna. Pellentesque pretium orci a felis tincidunt, sit amet volutpat est dapibus. Donec laoreet, massa in imperdiet laoreet, enim ligula auctor est, non imperdiet nisi diam vitae quam. Integer nec porttitor ante. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.Morbi non pretium risus, a lobortis eros. Etiam blandit diam tortor. Ut feugiat eros id erat auctor, ut vehicula odio vestibulum.</p>
                                       <p>Nunc sed ex sed diam euismod eleifend. Proin blandit lorem sed placerat fermentum. Curabitur non gravida felis, ac sollicitudin nibh. Morbi ornare sapien vitae nisl condimentum cursus.Vivamus bibendum condimentum metus, ut gravida orci bibendum maximus. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.Duis varius, tortor ac placerat faucibus, lectus mauris bibendum elit, id eleifend leo diam ac nulla.Aenean egestas urna facilisis purus ullamcorper vestibulum.Etiam commodo suscipit turpis, quis lobortis metus posuere sed.</p>
                                       <p>Praesent in augue sit amet tortor ultricies maximus eu ac dui.Pellentesque et congue elit. Suspendisse potenti. Donec facilisis eu magna nec bibendum. Nullam in dignissim elit. Integer laoreet odio massa, vel vestibulum mauris varius et. Ut non ex sit amet nisl mollis laoreet. </p> ";

            var page = new Page
            {
                Id = 1,
                VanityId = Guid.NewGuid(),
                Title = "Sample page",
                Slug = "sample-page",
                Description = "This is a sample page from a sample website",
                Status = DocumentStatus.Published,
                PublishedOn = DateTime.Now,
                DocumentContent = documentContent,
                WebsiteId = website.Id
            };

            var post = new Post
            {
                Id = 1,
                VanityId = Guid.NewGuid(),
                Title = "Lorem Ipsum",
                Slug = "lorem-ipsum",
                Description = "Lorem ipsum dolor sit amet",
                Status = DocumentStatus.Published,
                PublishedOn = DateTime.Now,
                DocumentContent = documentContent,
                WebsiteId = website.Id
            };

            var category = new Category
            {
                Id = 1,
                VanityId = Guid.NewGuid(),
                Name = "Category example",
                Slug = "category-example",
                WebsiteId = website.Id
            };

            var postCategory = new PostCategory
            {
                CategoryId = category.Id,
                PostId = post.Id
            };

            var postApplicationUser = new PostApplicationUser
            {
                ApplicationUserId = applicationUser.Id,
                PostId = post.Id
            };

            var pageApplicationUser = new PageApplicationUser
            {
                ApplicationUserId = applicationUser.Id,
                PageId = post.Id
            };

            builder.Entity<Website>().HasData(website);
            builder.Entity<Page>().HasData(page);
            builder.Entity<Post>().HasData(post);
            builder.Entity<Category>().HasData(category);
            builder.Entity<PostCategory>().HasData(postCategory);
            builder.Entity<ApplicationUser>().HasData(applicationUser);
            builder.Entity<PostApplicationUser>().HasData(postApplicationUser);
            builder.Entity<PageApplicationUser>().HasData(pageApplicationUser);
        }
    }
}

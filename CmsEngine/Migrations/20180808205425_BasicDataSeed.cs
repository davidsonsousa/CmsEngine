using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CmsEngine.Migrations
{
    public partial class BasicDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f41b40c2-8ef5-4006-aab1-2e8cdf53fc8a", 0, "e290f882-e3eb-427b-b32f-f87628ae0f83", "john@doe.com", true, false, null, "John", "JOHN@DOE.COM", "JOHN@DOE.COM", "AQAAAAEAACcQAAAAEGIUaLe7RWZGw8Tr5/xoUMOooAzJsLFw550fDqZkrbk8CD+urHQzYjK1xY8vcDMekw==", null, false, "NBTDBYKTNLGHKQ3HI7YFEHPQN5YRXWQC", "Doe", false, "john@doe.com" });

            migrationBuilder.InsertData(
                table: "Websites",
                columns: new[] { "Id", "Address", "ArticleLimit", "Culture", "DateCreated", "DateFormat", "DateModified", "Description", "Email", "Facebook", "Instagram", "IsDeleted", "LinkedIn", "Name", "Phone", "SiteUrl", "Twitter", "UrlFormat", "UserCreated", "UserModified", "VanityId" },
                values: new object[] { 1, null, 10, "en-US", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "MM/dd/yyyy", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "This is a sample website", null, null, null, false, null, "Sample Website", null, "cmsengine.test", null, "http://[site_url]/[type]/[slug]", null, null, new Guid("2291eacf-89bb-46e7-a81b-1c36dfd7fac5") });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DateCreated", "DateModified", "Description", "IsDeleted", "Name", "Slug", "UserCreated", "UserModified", "VanityId", "WebsiteId" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Category example", "category-example", null, null, new Guid("8632fdbf-93a1-46fc-9fa5-ebea32b36dcb"), 1 });

            migrationBuilder.InsertData(
                table: "Pages",
                columns: new[] { "Id", "Author", "AuthorId", "DateCreated", "DateModified", "Description", "DocumentContent", "IsDeleted", "PublishedOn", "Slug", "Status", "Title", "UserCreated", "UserModified", "VanityId", "WebsiteId" },
                values: new object[] { 1, "john@doe.com", "f41b40c2-8ef5-4006-aab1-2e8cdf53fc8a", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "This is a sample page from a sample website", @"<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed risus libero, egestas vel tempus id, venenatis nec tellus. Nullam hendrerit id magna quis venenatis. Pellentesque rhoncus leo vitae turpis tristique, nec placerat tellus scelerisque. Aenean vitae rhoncus urna, non posuere elit. Nullam quam libero, porttitor in lectus convallis, pellentesque finibus libero. Suspendisse potenti. Fusce quis purus egestas, malesuada massa sed, dignissim purus. Curabitur vitae rhoncus nulla, sit amet dignissim quam.</p>
                                       <p>Mauris lorem urna, convallis in enim nec, tristique ullamcorper nisl. Fusce nec tellus et arcu imperdiet ullamcorper vestibulum vitae mi. Sed bibendum molestie dolor sit amet rhoncus.Duis consectetur convallis auctor. In hac habitasse platea dictumst.Duis lorem nibh, mattis ut purus interdum, scelerisque molestie est. Nullam molestie a est vel ornare. Maecenas rhoncus accumsan ligula, at pretium purus tempus ut. Aliquam erat nulla, pretium vel eros vitae, blandit aliquam nibh. Nulla tincidunt, justo et ullamcorper dictum, augue lectus dictum ligula, eget rutrum sem nibh non felis.Aenean elementum, sem sit amet pulvinar tempus, neque eros faucibus turpis, quis molestie nisi libero quis purus.</p>
                                       <p>Donec quam massa, tincidunt eu lacus in, lacinia hendrerit urna. Pellentesque pretium orci a felis tincidunt, sit amet volutpat est dapibus. Donec laoreet, massa in imperdiet laoreet, enim ligula auctor est, non imperdiet nisi diam vitae quam. Integer nec porttitor ante. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.Morbi non pretium risus, a lobortis eros. Etiam blandit diam tortor. Ut feugiat eros id erat auctor, ut vehicula odio vestibulum.</p>
                                       <p>Nunc sed ex sed diam euismod eleifend. Proin blandit lorem sed placerat fermentum. Curabitur non gravida felis, ac sollicitudin nibh. Morbi ornare sapien vitae nisl condimentum cursus.Vivamus bibendum condimentum metus, ut gravida orci bibendum maximus. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.Duis varius, tortor ac placerat faucibus, lectus mauris bibendum elit, id eleifend leo diam ac nulla.Aenean egestas urna facilisis purus ullamcorper vestibulum.Etiam commodo suscipit turpis, quis lobortis metus posuere sed.</p>
                                       <p>Praesent in augue sit amet tortor ultricies maximus eu ac dui.Pellentesque et congue elit. Suspendisse potenti. Donec facilisis eu magna nec bibendum. Nullam in dignissim elit. Integer laoreet odio massa, vel vestibulum mauris varius et. Ut non ex sit amet nisl mollis laoreet. </p> ", false, new DateTime(2018, 8, 8, 22, 54, 25, 206, DateTimeKind.Local), "sample-page", 0, "Sample page", null, null, new Guid("9fbd9f8a-7160-4cf8-8287-15504d1d7a95"), 1 });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Author", "AuthorId", "DateCreated", "DateModified", "Description", "DocumentContent", "IsDeleted", "PublishedOn", "Slug", "Status", "Title", "UserCreated", "UserModified", "VanityId", "WebsiteId" },
                values: new object[] { 1, "john@doe.com", "f41b40c2-8ef5-4006-aab1-2e8cdf53fc8a", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lorem ipsum dolor sit amet", @"<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed risus libero, egestas vel tempus id, venenatis nec tellus. Nullam hendrerit id magna quis venenatis. Pellentesque rhoncus leo vitae turpis tristique, nec placerat tellus scelerisque. Aenean vitae rhoncus urna, non posuere elit. Nullam quam libero, porttitor in lectus convallis, pellentesque finibus libero. Suspendisse potenti. Fusce quis purus egestas, malesuada massa sed, dignissim purus. Curabitur vitae rhoncus nulla, sit amet dignissim quam.</p>
                                       <p>Mauris lorem urna, convallis in enim nec, tristique ullamcorper nisl. Fusce nec tellus et arcu imperdiet ullamcorper vestibulum vitae mi. Sed bibendum molestie dolor sit amet rhoncus.Duis consectetur convallis auctor. In hac habitasse platea dictumst.Duis lorem nibh, mattis ut purus interdum, scelerisque molestie est. Nullam molestie a est vel ornare. Maecenas rhoncus accumsan ligula, at pretium purus tempus ut. Aliquam erat nulla, pretium vel eros vitae, blandit aliquam nibh. Nulla tincidunt, justo et ullamcorper dictum, augue lectus dictum ligula, eget rutrum sem nibh non felis.Aenean elementum, sem sit amet pulvinar tempus, neque eros faucibus turpis, quis molestie nisi libero quis purus.</p>
                                       <p>Donec quam massa, tincidunt eu lacus in, lacinia hendrerit urna. Pellentesque pretium orci a felis tincidunt, sit amet volutpat est dapibus. Donec laoreet, massa in imperdiet laoreet, enim ligula auctor est, non imperdiet nisi diam vitae quam. Integer nec porttitor ante. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.Morbi non pretium risus, a lobortis eros. Etiam blandit diam tortor. Ut feugiat eros id erat auctor, ut vehicula odio vestibulum.</p>
                                       <p>Nunc sed ex sed diam euismod eleifend. Proin blandit lorem sed placerat fermentum. Curabitur non gravida felis, ac sollicitudin nibh. Morbi ornare sapien vitae nisl condimentum cursus.Vivamus bibendum condimentum metus, ut gravida orci bibendum maximus. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.Duis varius, tortor ac placerat faucibus, lectus mauris bibendum elit, id eleifend leo diam ac nulla.Aenean egestas urna facilisis purus ullamcorper vestibulum.Etiam commodo suscipit turpis, quis lobortis metus posuere sed.</p>
                                       <p>Praesent in augue sit amet tortor ultricies maximus eu ac dui.Pellentesque et congue elit. Suspendisse potenti. Donec facilisis eu magna nec bibendum. Nullam in dignissim elit. Integer laoreet odio massa, vel vestibulum mauris varius et. Ut non ex sit amet nisl mollis laoreet. </p> ", false, new DateTime(2018, 8, 8, 22, 54, 25, 207, DateTimeKind.Local), "lorem-ipsum", 0, "Lorem Ipsum", null, null, new Guid("8190711a-1771-4ae0-8ee1-5b20a97f68cd"), 1 });

            migrationBuilder.InsertData(
                table: "PostCategory",
                columns: new[] { "PostId", "CategoryId" },
                values: new object[] { 1, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "f41b40c2-8ef5-4006-aab1-2e8cdf53fc8a", "e290f882-e3eb-427b-b32f-f87628ae0f83" });

            migrationBuilder.DeleteData(
                table: "Pages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PostCategory",
                keyColumns: new[] { "PostId", "CategoryId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Websites",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}

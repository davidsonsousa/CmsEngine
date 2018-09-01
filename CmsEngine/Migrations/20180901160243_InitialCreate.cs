using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CmsEngine.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Websites",
                columns: table => new
                {
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VanityId = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    UserCreated = table.Column<string>(maxLength: 20, nullable: true),
                    UserModified = table.Column<string>(maxLength: 20, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    Culture = table.Column<string>(maxLength: 5, nullable: false),
                    UrlFormat = table.Column<string>(maxLength: 100, nullable: false),
                    DateFormat = table.Column<string>(maxLength: 10, nullable: false),
                    SiteUrl = table.Column<string>(maxLength: 250, nullable: false),
                    ArticleLimit = table.Column<int>(nullable: false),
                    Address = table.Column<string>(maxLength: 250, nullable: true),
                    Phone = table.Column<string>(maxLength: 20, nullable: true),
                    Email = table.Column<string>(maxLength: 250, nullable: true),
                    Facebook = table.Column<string>(maxLength: 20, nullable: true),
                    Twitter = table.Column<string>(maxLength: 20, nullable: true),
                    Instagram = table.Column<string>(maxLength: 20, nullable: true),
                    LinkedIn = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Websites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VanityId = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    UserCreated = table.Column<string>(maxLength: 20, nullable: true),
                    UserModified = table.Column<string>(maxLength: 20, nullable: true),
                    WebsiteId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 25, nullable: false),
                    Slug = table.Column<string>(maxLength: 25, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Websites_WebsiteId",
                        column: x => x.WebsiteId,
                        principalTable: "Websites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VanityId = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    UserCreated = table.Column<string>(maxLength: 20, nullable: true),
                    UserModified = table.Column<string>(maxLength: 20, nullable: true),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Slug = table.Column<string>(maxLength: 25, nullable: false),
                    Description = table.Column<string>(maxLength: 150, nullable: false),
                    DocumentContent = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    PublishedOn = table.Column<DateTime>(nullable: false),
                    WebsiteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_Websites_WebsiteId",
                        column: x => x.WebsiteId,
                        principalTable: "Websites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VanityId = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    UserCreated = table.Column<string>(maxLength: 20, nullable: true),
                    UserModified = table.Column<string>(maxLength: 20, nullable: true),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Slug = table.Column<string>(maxLength: 25, nullable: false),
                    Description = table.Column<string>(maxLength: 150, nullable: false),
                    DocumentContent = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    PublishedOn = table.Column<DateTime>(nullable: false),
                    WebsiteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Websites_WebsiteId",
                        column: x => x.WebsiteId,
                        principalTable: "Websites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VanityId = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    UserCreated = table.Column<string>(maxLength: 20, nullable: true),
                    UserModified = table.Column<string>(maxLength: 20, nullable: true),
                    WebsiteId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 25, nullable: false),
                    Slug = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Websites_WebsiteId",
                        column: x => x.WebsiteId,
                        principalTable: "Websites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageAspNetUser",
                columns: table => new
                {
                    PageId = table.Column<int>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageAspNetUser", x => new { x.PageId, x.ApplicationUserId });
                    table.ForeignKey(
                        name: "FK_PageAspNetUser_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PageAspNetUser_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PostAspNetUser",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostAspNetUser", x => new { x.PostId, x.ApplicationUserId });
                    table.ForeignKey(
                        name: "FK_PostAspNetUser_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PostAspNetUser_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PostCategory",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCategory", x => new { x.PostId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_PostCategory_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PostCategory_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PostTag",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTag", x => new { x.PostId, x.TagId });
                    table.ForeignKey(
                        name: "FK_PostTag_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PostTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0dca330d-37a1-44e9-af4f-db9f976ee2ae", 0, "0e2523ab-868d-43c6-bc0d-e695786d8809", "john@doe.com", true, false, null, "John", "JOHN@DOE.COM", "JOHN@DOE.COM", "AQAAAAEAACcQAAAAEGIUaLe7RWZGw8Tr5/xoUMOooAzJsLFw550fDqZkrbk8CD+urHQzYjK1xY8vcDMekw==", null, false, "NBTDBYKTNLGHKQ3HI7YFEHPQN5YRXWQC", "Doe", false, "john@doe.com" });

            migrationBuilder.InsertData(
                table: "Websites",
                columns: new[] { "Id", "Address", "ArticleLimit", "Culture", "DateCreated", "DateFormat", "DateModified", "Description", "Email", "Facebook", "Instagram", "IsDeleted", "LinkedIn", "Name", "Phone", "SiteUrl", "Twitter", "UrlFormat", "UserCreated", "UserModified", "VanityId" },
                values: new object[] { 1, null, 10, "en-US", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "MM/dd/yyyy", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "This is a sample website", null, null, null, false, null, "Sample Website", null, "cmsengine.test", null, "http://[site_url]/[type]/[slug]", null, null, new Guid("446952de-3e52-4bfa-8481-a19be00782b7") });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DateCreated", "DateModified", "Description", "IsDeleted", "Name", "Slug", "UserCreated", "UserModified", "VanityId", "WebsiteId" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Category example", "category-example", null, null, new Guid("1e6229ee-e0e4-4bfc-8e9e-d332c2a4bdb0"), 1 });

            migrationBuilder.InsertData(
                table: "Pages",
                columns: new[] { "Id", "DateCreated", "DateModified", "Description", "DocumentContent", "IsDeleted", "PublishedOn", "Slug", "Status", "Title", "UserCreated", "UserModified", "VanityId", "WebsiteId" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "This is a sample page from a sample website", @"<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed risus libero, egestas vel tempus id, venenatis nec tellus. Nullam hendrerit id magna quis venenatis. Pellentesque rhoncus leo vitae turpis tristique, nec placerat tellus scelerisque. Aenean vitae rhoncus urna, non posuere elit. Nullam quam libero, porttitor in lectus convallis, pellentesque finibus libero. Suspendisse potenti. Fusce quis purus egestas, malesuada massa sed, dignissim purus. Curabitur vitae rhoncus nulla, sit amet dignissim quam.</p>
                                       <p>Mauris lorem urna, convallis in enim nec, tristique ullamcorper nisl. Fusce nec tellus et arcu imperdiet ullamcorper vestibulum vitae mi. Sed bibendum molestie dolor sit amet rhoncus.Duis consectetur convallis auctor. In hac habitasse platea dictumst.Duis lorem nibh, mattis ut purus interdum, scelerisque molestie est. Nullam molestie a est vel ornare. Maecenas rhoncus accumsan ligula, at pretium purus tempus ut. Aliquam erat nulla, pretium vel eros vitae, blandit aliquam nibh. Nulla tincidunt, justo et ullamcorper dictum, augue lectus dictum ligula, eget rutrum sem nibh non felis.Aenean elementum, sem sit amet pulvinar tempus, neque eros faucibus turpis, quis molestie nisi libero quis purus.</p>
                                       <p>Donec quam massa, tincidunt eu lacus in, lacinia hendrerit urna. Pellentesque pretium orci a felis tincidunt, sit amet volutpat est dapibus. Donec laoreet, massa in imperdiet laoreet, enim ligula auctor est, non imperdiet nisi diam vitae quam. Integer nec porttitor ante. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.Morbi non pretium risus, a lobortis eros. Etiam blandit diam tortor. Ut feugiat eros id erat auctor, ut vehicula odio vestibulum.</p>
                                       <p>Nunc sed ex sed diam euismod eleifend. Proin blandit lorem sed placerat fermentum. Curabitur non gravida felis, ac sollicitudin nibh. Morbi ornare sapien vitae nisl condimentum cursus.Vivamus bibendum condimentum metus, ut gravida orci bibendum maximus. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.Duis varius, tortor ac placerat faucibus, lectus mauris bibendum elit, id eleifend leo diam ac nulla.Aenean egestas urna facilisis purus ullamcorper vestibulum.Etiam commodo suscipit turpis, quis lobortis metus posuere sed.</p>
                                       <p>Praesent in augue sit amet tortor ultricies maximus eu ac dui.Pellentesque et congue elit. Suspendisse potenti. Donec facilisis eu magna nec bibendum. Nullam in dignissim elit. Integer laoreet odio massa, vel vestibulum mauris varius et. Ut non ex sit amet nisl mollis laoreet. </p> ", false, new DateTime(2018, 9, 1, 18, 2, 43, 318, DateTimeKind.Local), "sample-page", 0, "Sample page", null, null, new Guid("88efcb57-6af9-4766-879e-beefff98dd05"), 1 });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "DateCreated", "DateModified", "Description", "DocumentContent", "IsDeleted", "PublishedOn", "Slug", "Status", "Title", "UserCreated", "UserModified", "VanityId", "WebsiteId" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lorem ipsum dolor sit amet", @"<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed risus libero, egestas vel tempus id, venenatis nec tellus. Nullam hendrerit id magna quis venenatis. Pellentesque rhoncus leo vitae turpis tristique, nec placerat tellus scelerisque. Aenean vitae rhoncus urna, non posuere elit. Nullam quam libero, porttitor in lectus convallis, pellentesque finibus libero. Suspendisse potenti. Fusce quis purus egestas, malesuada massa sed, dignissim purus. Curabitur vitae rhoncus nulla, sit amet dignissim quam.</p>
                                       <p>Mauris lorem urna, convallis in enim nec, tristique ullamcorper nisl. Fusce nec tellus et arcu imperdiet ullamcorper vestibulum vitae mi. Sed bibendum molestie dolor sit amet rhoncus.Duis consectetur convallis auctor. In hac habitasse platea dictumst.Duis lorem nibh, mattis ut purus interdum, scelerisque molestie est. Nullam molestie a est vel ornare. Maecenas rhoncus accumsan ligula, at pretium purus tempus ut. Aliquam erat nulla, pretium vel eros vitae, blandit aliquam nibh. Nulla tincidunt, justo et ullamcorper dictum, augue lectus dictum ligula, eget rutrum sem nibh non felis.Aenean elementum, sem sit amet pulvinar tempus, neque eros faucibus turpis, quis molestie nisi libero quis purus.</p>
                                       <p>Donec quam massa, tincidunt eu lacus in, lacinia hendrerit urna. Pellentesque pretium orci a felis tincidunt, sit amet volutpat est dapibus. Donec laoreet, massa in imperdiet laoreet, enim ligula auctor est, non imperdiet nisi diam vitae quam. Integer nec porttitor ante. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.Morbi non pretium risus, a lobortis eros. Etiam blandit diam tortor. Ut feugiat eros id erat auctor, ut vehicula odio vestibulum.</p>
                                       <p>Nunc sed ex sed diam euismod eleifend. Proin blandit lorem sed placerat fermentum. Curabitur non gravida felis, ac sollicitudin nibh. Morbi ornare sapien vitae nisl condimentum cursus.Vivamus bibendum condimentum metus, ut gravida orci bibendum maximus. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.Duis varius, tortor ac placerat faucibus, lectus mauris bibendum elit, id eleifend leo diam ac nulla.Aenean egestas urna facilisis purus ullamcorper vestibulum.Etiam commodo suscipit turpis, quis lobortis metus posuere sed.</p>
                                       <p>Praesent in augue sit amet tortor ultricies maximus eu ac dui.Pellentesque et congue elit. Suspendisse potenti. Donec facilisis eu magna nec bibendum. Nullam in dignissim elit. Integer laoreet odio massa, vel vestibulum mauris varius et. Ut non ex sit amet nisl mollis laoreet. </p> ", false, new DateTime(2018, 9, 1, 18, 2, 43, 318, DateTimeKind.Local), "lorem-ipsum", 0, "Lorem Ipsum", null, null, new Guid("3c2c179d-61bb-46fb-bbc4-f7deeb91d151"), 1 });

            migrationBuilder.InsertData(
                table: "PageAspNetUser",
                columns: new[] { "PageId", "ApplicationUserId" },
                values: new object[] { 1, "0dca330d-37a1-44e9-af4f-db9f976ee2ae" });

            migrationBuilder.InsertData(
                table: "PostAspNetUser",
                columns: new[] { "PostId", "ApplicationUserId" },
                values: new object[] { 1, "0dca330d-37a1-44e9-af4f-db9f976ee2ae" });

            migrationBuilder.InsertData(
                table: "PostCategory",
                columns: new[] { "PostId", "CategoryId" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_WebsiteId",
                table: "Categories",
                column: "WebsiteId");

            migrationBuilder.CreateIndex(
                name: "IX_PageAspNetUser_ApplicationUserId",
                table: "PageAspNetUser",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_WebsiteId",
                table: "Pages",
                column: "WebsiteId");

            migrationBuilder.CreateIndex(
                name: "IX_PostAspNetUser_ApplicationUserId",
                table: "PostAspNetUser",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostCategory_CategoryId",
                table: "PostCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_WebsiteId",
                table: "Posts",
                column: "WebsiteId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_TagId",
                table: "PostTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_WebsiteId",
                table: "Tags",
                column: "WebsiteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "PageAspNetUser");

            migrationBuilder.DropTable(
                name: "PostAspNetUser");

            migrationBuilder.DropTable(
                name: "PostCategory");

            migrationBuilder.DropTable(
                name: "PostTag");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Websites");
        }
    }
}

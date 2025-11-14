namespace CmsEngine.Application.Extensions.Mapper;

public static class PostExtensions
{
    extension(Post item)
    {
        /// <summary>
        /// Maps Post model into a PostEditModel
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public PostEditModel MapToEditModel()
        {
            return new PostEditModel
            {
                Id = item.Id,
                VanityId = item.VanityId,
                Title = item.Title,
                Slug = item.Slug,
                Description = item.Description,
                DocumentContent = item.DocumentContent,
                HeaderImage = item.HeaderImage,
                PublishedOn = item.PublishedOn,
                Status = item.Status,
                SelectedCategories = item.PostCategories.Select(x => x.Category.VanityId.ToString()),
                SelectedTags = item.PostTags.Select(x => x.Tag.VanityId.ToString())
            };
        }

        /// <summary>
        /// Maps Post model into a PostViewModel
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public PostViewModel MapToViewModel(string dateFormat)
        {
            var formattedDate = item.PublishedOn.ToString(dateFormat);

            var authorUser = item.ApplicationUsers?.FirstOrDefault();
            var author = authorUser is null ? new UserViewModel() : new UserViewModel
            {
                Name = authorUser.Name,
                Surname = authorUser.Surname,
                Email = authorUser.Email,
                UserName = authorUser.UserName
            };

            return new PostViewModel
            {
                VanityId = item.VanityId,
                Title = item.Title,
                Slug = item.Slug,
                Description = item.Description,
                DocumentContent = item.DocumentContent,
                HeaderImage = item.HeaderImage,
                PublishedOn = formattedDate,
                Categories = item.Categories.MapToViewModelSimple(),
                Author = author
            };
        }

        /// <summary>
        /// Maps a Post model into a PostTableViewModel for display in a table
        /// </summary>
        /// <param name="item">The Post item to map</param>
        /// <param name="dateFormat">The date format for displaying dates</param>
        /// <returns>A PostTableViewModel with the mapped data</returns>
        public PostTableViewModel MapToTableViewModel(string dateFormat)
        {
            var formattedDate = item.PublishedOn.ToString(dateFormat);

            var authorUser = item.ApplicationUsers?.FirstOrDefault();
            var author = authorUser is null ? new UserViewModel() : new UserViewModel
            {
                Name = authorUser.Name,
                Surname = authorUser.Surname,
                Email = authorUser.Email,
                UserName = authorUser.UserName
            };

            return new PostTableViewModel
            {
                VanityId = item.VanityId,
                Title = item.Title,
                Description = item.Description,
                Slug = item.Slug,
                PublishedOn = formattedDate,
                Status = item.Status,
                Author = author
            };
        }
    }

    extension(PostEditModel item)
    {
        /// <summary>
        /// Maps a PostEditModel into a Post
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Post MapToModel()
        {
            return new Post
            {
                Id = item.Id,
                VanityId = item.VanityId,
                Title = item.Title,
                Slug = item.Slug,
                Description = item.Description,
                DocumentContent = item.DocumentContent,
                HeaderImage = item.HeaderImage,
                PublishedOn = item.PublishedOn,
                Status = item.Status
            };
        }

        /// <summary>
        /// Maps a PostEditModel into a specific Post
        /// </summary>
        /// <param name="item"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        public Post MapToModel(Post post)
        {
            post.Id = item.Id;
            post.VanityId = item.VanityId;
            post.Title = item.Title;
            post.Slug = item.Slug;
            post.Description = item.Description;
            post.DocumentContent = item.DocumentContent;
            post.HeaderImage = item.HeaderImage;
            post.PublishedOn = item.PublishedOn;
            post.Status = item.Status;

            return post;
        }
    }

    extension(IEnumerable<Post> posts)
    {
        /// <summary>
        /// Maps an IEnumerable<Post> into an IEnumerable<PostEditModel>
        /// </summary>
        /// <param name="posts"></param>
        /// <returns></returns>
        public IEnumerable<PostEditModel> MapToEditModel()
        {
            return posts.Select(item => new PostEditModel
            {
                Id = item.Id,
                VanityId = item.VanityId,
                Title = item.Title,
                Slug = item.Slug,
                Description = item.Description,
                DocumentContent = item.DocumentContent,
                HeaderImage = item.HeaderImage,
                PublishedOn = item.PublishedOn,
                Status = item.Status
            }).ToList();
        }

        /// <summary>
        /// Maps Post model into a PostViewModel
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IEnumerable<PostViewModel> MapToViewModel(string dateFormat)
        {
            var list = new List<PostViewModel>();
            foreach (var item in posts)
            {
                var formattedDate = item.PublishedOn.ToString(dateFormat);
                list.Add(new PostViewModel
                {
                    Id = item.Id,
                    VanityId = item.VanityId,
                    Title = item.Title,
                    Slug = item.Slug,
                    Description = item.Description,
                    DocumentContent = item.DocumentContent,
                    HeaderImage = item.HeaderImage,
                    PublishedOn = formattedDate,
                    Status = item.Status
                });
            }

            return list;
        }

        /// <summary>
        /// Maps Post model into a PostViewModel with Categories
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IEnumerable<PostViewModel> MapToViewModelWithCategories(string dateFormat)
        {
            var list = new List<PostViewModel>();
            var categoryCache = new Dictionary<Guid, CategoryViewModel>();

            foreach (var item in posts)
            {
                var formattedDate = item.PublishedOn.ToString(dateFormat);

                var categories = new List<CategoryViewModel>();
                foreach (var pc in item.PostCategories)
                {
                    var cat = pc.Category;
                    if (!categoryCache.TryGetValue(cat.VanityId, out var catVm))
                    {
                        catVm = new CategoryViewModel
                        {
                            Id = cat.Id,
                            VanityId = cat.VanityId,
                            Name = cat.Name,
                            Slug = cat.Slug
                        };
                        categoryCache[cat.VanityId] = catVm;
                    }
                    categories.Add(catVm);
                }

                list.Add(new PostViewModel
                {
                    Id = item.Id,
                    VanityId = item.VanityId,
                    Title = item.Title,
                    Slug = item.Slug,
                    Description = item.Description,
                    DocumentContent = item.DocumentContent,
                    HeaderImage = item.HeaderImage,
                    PublishedOn = formattedDate,
                    Status = item.Status,
                    Categories = categories
                });
            }

            return list;
        }

        /// <summary>
        /// Maps Post model into a PostViewModel with Tags
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IEnumerable<PostViewModel> MapToViewModelWithTags(string dateFormat)
        {
            var list = new List<PostViewModel>();
            var tagCache = new Dictionary<Guid, TagViewModel>();

            foreach (var item in posts)
            {
                var formattedDate = item.PublishedOn.ToString(dateFormat);

                var tags = new List<TagViewModel>();
                foreach (var pt in item.PostTags)
                {
                    var tag = pt.Tag;
                    if (!tagCache.TryGetValue(tag.VanityId, out var tagVm))
                    {
                        tagVm = new TagViewModel
                        {
                            Id = tag.Id,
                            VanityId = tag.VanityId,
                            Name = tag.Name,
                            Slug = tag.Slug
                        };
                        tagCache[tag.VanityId] = tagVm;
                    }
                    tags.Add(tagVm);
                }

                list.Add(new PostViewModel
                {
                    Id = item.Id,
                    VanityId = item.VanityId,
                    Title = item.Title,
                    Slug = item.Slug,
                    Description = item.Description,
                    DocumentContent = item.DocumentContent,
                    HeaderImage = item.HeaderImage,
                    PublishedOn = formattedDate,
                    Status = item.Status,
                    Tags = tags
                });
            }

            return list;
        }

        /// <summary>
        /// Maps limited information for Partial Views
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IEnumerable<PostViewModel> MapToViewModelForPartialView(string dateFormat)
        {
            var list = new List<PostViewModel>();
            var userCache = new Dictionary<string, UserViewModel>();

            foreach (var item in posts)
            {
                var formattedDate = item.PublishedOn.ToString(dateFormat);

                var authorUser = item.ApplicationUsers?.FirstOrDefault();
                var author = new UserViewModel();
                if (authorUser is not null)
                {
                    if (!userCache.TryGetValue(authorUser.Id, out author))
                    {
                        author = new UserViewModel
                        {
                            Name = authorUser.Name,
                            Surname = authorUser.Surname,
                            Email = authorUser.Email,
                            UserName = authorUser.UserName
                        };
                        userCache[authorUser.Id] = author;
                    }
                }

                list.Add(new PostViewModel
                {
                    VanityId = item.VanityId,
                    Title = item.Title,
                    Slug = item.Slug,
                    Description = item.Description,
                    HeaderImage = item.HeaderImage,
                    PublishedOn = formattedDate,
                    Categories = item.Categories.MapToViewModelSimple(),
                    Author = author
                });
            }

            return list;
        }

        /// <summary>
        /// Maps limited information for Partial Views for Tags
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IEnumerable<PostViewModel> MapToViewModelForPartialViewForTags(string dateFormat)
        {
            var list = new List<PostViewModel>();
            var userCache = new Dictionary<string, UserViewModel>();
            var categoryCache = new Dictionary<Guid, CategoryViewModel>();
            var tagCache = new Dictionary<Guid, TagViewModel>();

            foreach (var item in posts)
            {
                var formattedDate = item.PublishedOn.ToString(dateFormat);

                var authorUser = item.ApplicationUsers?.FirstOrDefault();
                var author = new UserViewModel();
                if (authorUser is not null)
                {
                    if (!userCache.TryGetValue(authorUser.Id, out author))
                    {
                        author = new UserViewModel
                        {
                            Name = authorUser.Name,
                            Surname = authorUser.Surname,
                            Email = authorUser.Email,
                            UserName = authorUser.UserName
                        };
                        userCache[authorUser.Id] = author;
                    }
                }

                var categories = new List<CategoryViewModel>();
                foreach (var c in item.Categories)
                {
                    if (!categoryCache.TryGetValue(c.VanityId, out var cVm))
                    {
                        cVm = new CategoryViewModel { Id = c.Id, VanityId = c.VanityId, Name = c.Name, Slug = c.Slug };
                        categoryCache[c.VanityId] = cVm;
                    }
                    categories.Add(cVm);
                }

                var tags = new List<TagViewModel>();
                foreach (var t in item.Tags)
                {
                    if (!tagCache.TryGetValue(t.VanityId, out var tVm))
                    {
                        tVm = new TagViewModel { Id = t.Id, VanityId = t.VanityId, Name = t.Name, Slug = t.Slug };
                        tagCache[t.VanityId] = tVm;
                    }
                    tags.Add(tVm);
                }

                list.Add(new PostViewModel
                {
                    VanityId = item.VanityId,
                    Title = item.Title,
                    Slug = item.Slug,
                    Description = item.Description,
                    HeaderImage = item.HeaderImage,
                    PublishedOn = formattedDate,
                    Categories = categories,
                    Tags = tags,
                    Author = author
                });
            }

            return list;
        }

        /// <summary>
        /// Maps an IEnumerable<Post> into an IEnumerable<PostTableViewModel>
        /// </summary>
        /// <param name="posts"></param>
        /// <returns></returns>
        public IEnumerable<PostTableViewModel> MapToTableViewModel()
        {
            var list = new List<PostTableViewModel>();
            var userCache = new Dictionary<string, UserViewModel>();

            foreach (var item in posts)
            {
                var formattedDate = item.PublishedOn.ToString("yyyy-MM-dd HH:mm:ss");

                var authorUser = item.ApplicationUsers?.FirstOrDefault();
                var author = new UserViewModel();
                if (authorUser is not null)
                {
                    if (!userCache.TryGetValue(authorUser.Id, out author))
                    {
                        author = new UserViewModel
                        {
                            Name = authorUser.Name,
                            Surname = authorUser.Surname,
                            Email = authorUser.Email,
                            UserName = authorUser.UserName
                        };
                        userCache[authorUser.Id] = author;
                    }
                }

                list.Add(new PostTableViewModel
                {
                    VanityId = item.VanityId,
                    Title = item.Title,
                    Description = item.Description,
                    Slug = item.Slug,
                    PublishedOn = formattedDate,
                    Status = item.Status,
                    Author = author
                });
            }

            return list;
        }
    }
}
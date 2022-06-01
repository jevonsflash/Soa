using Soa.AuthorizedService.EntityFrameworkCore;
using Soa.Sample.AuthorizedService.Models;
using System;
using System.Linq;

namespace AuthorizedService.EntityFrameworkCore.Seed
{
    internal class InitialDbBuilder
    {
        private AuthorizedServiceDbContext context;

        public InitialDbBuilder(AuthorizedServiceDbContext context)
        {
            this.context = context;
        }

        internal void CreateMusic()
        {
            if (!this.context.Music.Any(c => c.Title=="不顾一切的爱"))
            {
                this.context.Add(new Music()
                {
                    Title="不顾一切的爱",
                    Artist="李圣杰",
                    Album="手放开",
                    Duration=new TimeSpan(0, 4, 27),
                    ReleaseDate=new DateTime(2004, 1, 1)
                });

            }
            if (!this.context.Music.Any(c => c.Title=="阴天快乐"))
            {
                this.context.Add(new Music()
                {
                    Title="阴天快乐",
                    Artist="陈奕迅",
                    Album="米.闪",
                    Duration=new TimeSpan(0, 3, 25),
                    ReleaseDate=new DateTime(2014, 1, 1)
                });

            }

            if (!this.context.Music.Any(c => c.Title=="可惜不是你"))
            {
                this.context.Add(new Music()
                {
                    Title="可惜不是你",
                    Artist="梁静茹",
                    Album="通往爱的路途",
                    Duration=new TimeSpan(0, 4, 45),
                    ReleaseDate=new DateTime(2005, 1, 1)
                });

            }

            if (!this.context.Music.Any(c => c.Title=="喜欢你"))
            {
                this.context.Add(new Music()
                {
                    Title="喜欢你",
                    Artist="G.E.M.邓紫棋",
                    Album="喜欢你",
                    Duration=new TimeSpan(0, 3, 55),
                    ReleaseDate=new DateTime(2014, 1, 1)
                });

            }

            if (!this.context.Music.Any(c => c.Title=="光あるもの"))
            {
                this.context.Add(new Music()
                {
                    Title="光あるもの",
                    Artist="大山百合香",
                    Album="夏のしずく",
                    Duration=new TimeSpan(0, 3, 47),
                    ReleaseDate=new DateTime(2009, 1, 1)
                });

            }

            if (!this.context.Music.Any(c => c.Title=="好好恋爱"))
            {
                this.context.Add(new Music()
                {
                    Title="好好恋爱",
                    Artist="邓丽欣,方力申",
                    Album="情歌集",
                    Duration=new TimeSpan(0, 3, 2),
                    ReleaseDate=new DateTime(2004, 1, 1)
                });

            }

            if (!this.context.Music.Any(c => c.Title=="江南"))
            {
                this.context.Add(new Music()
                {
                    Title="江南",
                    Artist="林俊杰",
                    Album="第二天堂",
                    Duration=new TimeSpan(0, 4, 28),
                    ReleaseDate=new DateTime(2003, 1, 1)
                });

            }

        }
   
        internal void CreateMovie()
        {
            if (!this.context.Movie.Any(c => c.Title=="28岁未成年"))
            {
                this.context.Add(new Movie()
                {
                    Title="28岁未成年",
                    Genre="爱情",
                    ReleaseDate=new DateTime(2016, 12, 19),
                    Price=49
                });

            }
            if (!this.context.Movie.Any(c => c.Title=="无人区"))
            {
                this.context.Add(new Movie()
                {
                    Title="无人区",
                    Genre="犯罪",
                    ReleaseDate=new DateTime(2013, 12, 3),
                    Price=59
                });
            }

            if (!this.context.Movie.Any(c => c.Title=="你好，李焕英"))
            {
                this.context.Add(new Movie()
                {
                    Title="你好，李焕英",
                    Genre="搞笑",
                    ReleaseDate=new DateTime(2021, 2, 12),
                    Price=45
                });
            }


        }

    }
}
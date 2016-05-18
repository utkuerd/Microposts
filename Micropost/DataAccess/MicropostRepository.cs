using Microposts.Helper;
using Microposts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Microposts.DataAccess
{
    [Authorize]
    public class MicropostRepository : IMicropostRepository
    {
        private ApplicationDbContext context;

        public MicropostRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public int Create(string content, ApplicationUser user)
        {
            var newM = new Micropost { Content = content, User = user, CreatedAt = DateTime.Now };
            context.Microposts.Add(newM);
            context.SaveChanges();
            return newM.Id;
        }

        public void Save()
        {
            context.SaveChangesAsync();
        }

        public IEnumerable<Micropost> FindByUser(int userId)
        {
            return context.Microposts.Where(mp => mp.User.Id == userId);
        }

        [SelfPostAuthorize]
        public void Delete(int micropostId)
        {
            var mp = context.Microposts.Find(micropostId);
            context.Microposts.Remove(mp);
        }

        public Micropost FindById(int micropostId)
        {
            return context.Microposts.Find(micropostId);
        }

        public void AttachPicture(int id, string imagePath)
        {
            var mp = context.Microposts.Find(id);
            mp.Image = imagePath;
        }
    }

    public interface IMicropostRepository
    {         
        int Create(string Content, ApplicationUser user);
        void Save();
        Micropost FindById(int id);
        void AttachPicture(int id, string imagePath);
        void Delete(int id);
    }
}
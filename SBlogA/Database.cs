using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using SBlogA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBlogA
{
    public static class Database
    {
        private const string SessionKey = "SimpleBlog.Database.SessionKey";
        private static ISessionFactory _sessionFactory;

        public static ISession Session
        {
            get {
                return (ISession)HttpContext.Current.Items[SessionKey];
            }
        }


        public static void Configure()
        {
            //configuration
            var config = new Configuration();
            config.Configure();

            //add mappings
            var mapper = new ModelMapper();
            mapper.AddMapping<UserMap>();
            mapper.AddMapping<RoleMap>();
            mapper.AddMapping<TagMap>();
            mapper.AddMapping<PostMap>();

            config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

            //creating sessionFactory
            _sessionFactory = config.BuildSessionFactory();

        }

        public static void OpenSession()
        {
            HttpContext.Current.Items[SessionKey] = _sessionFactory.OpenSession();
        }

        public static void CloseSession()
        {

            var session = HttpContext.Current.Items[SessionKey] as ISession;
            if (session != null)
            {
                session.Close();
            }

            HttpContext.Current.Items.Remove("Session");
            
        }
    }
}
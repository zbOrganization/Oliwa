using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace stu_position
{
    public class DBcomunicator
    {

        private DBcomunicator() { }
        private static DBcomunicator com=null;
        private static DBcontext ctx = null;


        public static DBcomunicator GetInstance()
        {
            if (com == null)
            {
                DBcontext f = new DBcontext();
                
                ctx= new DBcontext();
            
                com = new DBcomunicator();
            }
             
            return com;
        }
        public void save()
       {

           ctx.SaveChanges();
       }
        public void Add<T>(T val) where T : class
       {
        


            ctx.Set<T>().Add(val);
            ctx.SaveChanges();
        }
       
        public void Update<T>(Action<T> expresion, Func<T, bool> wherequery) where T : class
       {
           foreach(T val in ctx.Set<T>().Where(wherequery))
           {

               expresion(val);
           }
          

               ctx.SaveChanges();


       }
        public void Delete<T>(Func<T, bool> wherequery) where T : class
       {
           foreach (T val in ctx.Set<T>().Where(wherequery))
           {

              ctx.Set<T>().Remove(val);
           }
          ctx.SaveChanges();
       }
        public List<T> Select<T>(Func<T, bool> wherequery) where T: class
        {



      
       
        return ctx.Set<T>().Where(wherequery).ToList<T>();

            

            
        }       
        public List<T> Select<T>() where T : class
        {
            return ctx.Set<T>().ToList<T>();




        }
        public void Delete<T>() where T : class
        {

          foreach(T val in ctx.Set<T>())
          {
              ctx.Set<T>().Remove(val);
              

          }
          ctx.SaveChanges();
            

        }
        

     

    }
}
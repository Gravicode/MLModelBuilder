using ModelBuilder.Models;
using Microsoft.EntityFrameworkCore;
using ModelBuilder.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelBuilder.Web.Data
{
    public class MLModelService : ICrud<MLModel>
    {
        ModelBuilderDB db;

        public MLModelService()
        {
            if (db == null) db = new ModelBuilderDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.MLModels.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.MLModels.Remove(selData);
            db.SaveChanges();
            return true;
        }
        
        public List<MLModel> FindByKeyword(string Keyword)
        {
            var data = from x in db.MLModels
                       where x.Nama.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<MLModel> GetAllData()
        {
            return db.MLModels.ToList();
        }

        public MLModel GetDataById(object Id)
        {
            return db.MLModels.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(MLModel data)
        {
            try
            {
                db.MLModels.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(MLModel data)
        {
            try
            {
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();

                /*
                if (sel != null)
                {
                    sel.Nama = data.Nama;
                    sel.Keterangan = data.Keterangan;
                    sel.Tanggal = data.Tanggal;
                    sel.DocumentUrl = data.DocumentUrl;
                    sel.StreamUrl = data.StreamUrl;
                    return true;

                }*/
                return true;
            }
            catch
            {

            }
            return false;
        }

        public long GetLastId()
        {
            return db.MLModels.Max(x => x.Id);
        }
    }

}
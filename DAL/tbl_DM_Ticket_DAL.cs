﻿using DTO.Custom;
using DTO.tbl_DTO;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class tbl_DM_Ticket_DAL
    {
        private readonly string _connectionString = CConfig.CM_Cinema_DB_ConnectionString;
        public void AddData(tbl_DM_Ticket_DTO obj)
        {
            try
            {
                using (CM_Cinema_DBDataContext db = new CM_Cinema_DBDataContext(CConfig.CM_Cinema_DB_ConnectionString))
                {

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<tbl_DM_Ticket_DTO> GetList()
        {
            try
            {
                using (CM_Cinema_DBDataContext db = new CM_Cinema_DBDataContext(CConfig.CM_Cinema_DB_ConnectionString))
                {
                    List<tbl_DM_Ticket_DTO> list = new List<tbl_DM_Ticket_DTO>();
                    var result = from tk in db.tbl_DM_Tickets
                                 where tk.DELETED == 0
                                 select tk;
                    foreach (var item in result)
                    {
                        list.Add(new tbl_DM_Ticket_DTO(item.TK_AutoID, item.TK_SEATNAME, item.TK_MOVIESCHEDULE_AutoID, item.TK_STAFF_AutoID, (int)item.DELETED));
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void RemoveData(long ticketID)
        {
            try
            {
                using (CM_Cinema_DBDataContext db = new CM_Cinema_DBDataContext(CConfig.CM_Cinema_DB_ConnectionString))
                {
                    tbl_DM_Ticket foundTicket = db.tbl_DM_Tickets.SingleOrDefault(item => item.TK_AutoID == ticketID);
                    //objRes.DELETED = 1;
                    //objRes.UPDATED = DateTime.Now;
                    //objRes.UPDATED_BY = strUpdated_By;
                    //objRes.UPDATED_BY_FUNCTION = "Delete by ID";
                    //DBDataContext.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<tbl_DM_Ticket_DTO> GetList_ByMovieSchedule(long moviesSheduleID)
        {
            try
            {
                using (CM_Cinema_DBDataContext db = new CM_Cinema_DBDataContext(CConfig.CM_Cinema_DB_ConnectionString))
                {
                    List<tbl_DM_Ticket_DTO> list = new List<tbl_DM_Ticket_DTO>();
                    var result = from tk in db.tbl_DM_Tickets
                                 where tk.TK_MOVIESCHEDULE_AutoID == moviesSheduleID
                                 select tk;
                    foreach (var item in result)
                    {
                        list.Add(new tbl_DM_Ticket_DTO(item.TK_AutoID, item.TK_SEATNAME, item.TK_MOVIESCHEDULE_AutoID, item.TK_STAFF_AutoID, (int)item.DELETED));
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SeatExist_ByMovieSchedule(string seatName, long movieScheduleID)
        {
            using (CM_Cinema_DBDataContext db = new CM_Cinema_DBDataContext(CConfig.CM_Cinema_DB_ConnectionString))
            {
                return db.tbl_DM_Tickets.SingleOrDefault(item => item.TK_SEATNAME == seatName && item.TK_MOVIESCHEDULE_AutoID == movieScheduleID) != null;
            }
        }
        public tbl_DM_Ticket_DTO GetTicket_ByID(long ticketID)
        {
            try
            {
                using (CM_Cinema_DBDataContext db = new CM_Cinema_DBDataContext(CConfig.CM_Cinema_DB_ConnectionString))
                {
                    tbl_DM_Ticket foundTicket = db.tbl_DM_Tickets.SingleOrDefault(item => item.TK_AutoID == ticketID);
                    if(foundTicket != null)
                    {
                        tbl_DM_Ticket_DTO result = new tbl_DM_Ticket_DTO(foundTicket.TK_AutoID,foundTicket.TK_SEATNAME, foundTicket.TK_MOVIESCHEDULE_AutoID,foundTicket.TK_STAFF_AutoID, (int)foundTicket.DELETED);
                        return result;
                    }
                    else
                    {
                        throw new NullReferenceException("Không tìm thấy ID " + ticketID);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
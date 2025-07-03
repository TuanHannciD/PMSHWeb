using BaseBusiness.BO;
using BaseBusiness.Model;
using BaseBusiness.util;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Commons.Helpers
{
    public static class ListItemHelper
    {
        private static readonly string _textDefault = "Please choose";

        /// <summary>
        /// Lấy tất cả danh sách nationality inactive cho dropdown
        /// </summary>
        /// <param name="defaultValue">Giá trị mặc định.</param>
        /// <param name="textDefault">Text thứ hai.</param>
        /// <returns>Danh sách nationality</returns>
        public static List<SelectListItem> GetNationalityProvider(bool defaultValue = true, string textDefault = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textDefault)) textDefault = _textDefault + " Nationality";

                var items = new List<SelectListItem>();
                List<NationalityModel> list = PropertyUtils.ConvertToList<NationalityModel>(NationalityBO.Instance.FindByAttribute("Inactive", 0));
                if (list.Count > 0)
                {
                    items = list.Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.Name, Selected = false }).ToList();
                }
                if (defaultValue)
                    items.Insert(0, new SelectListItem { Text = textDefault, Value = string.Empty, Selected = true });

                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// Lấy tất cả danh sách Title inactive cho dropdown
        /// </summary>
        /// <param name="defaultValue">Giá trị mặc định.</param>
        /// <param name="textDefault">Text thứ hai.</param>
        /// <returns>Danh sách Title</returns>
        public static List<SelectListItem> GetTitleProvider(bool defaultValue = true, string textDefault = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textDefault)) textDefault = _textDefault + " Title";

                var items = new List<SelectListItem>();
                List<TitleModel> list = PropertyUtils.ConvertToList<TitleModel>(TitleBO.Instance.FindByAttribute("Inactive", 0));
                if (list.Count > 0)
                {
                    items = list.Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.Name, Selected = false }).ToList();
                }
                if (defaultValue)
                    items.Insert(0, new SelectListItem { Text = textDefault, Value = string.Empty, Selected = true });

                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// Lấy tất cả danh sách City inactive cho dropdown
        /// </summary>
        /// <param name="defaultValue">Giá trị mặc định.</param>
        /// <param name="textDefault">Text thứ hai.</param>
        /// <returns>Danh sách City</returns>
        public static List<SelectListItem> GetCityProvider(bool defaultValue = true, string textDefault = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textDefault)) textDefault = _textDefault + " City";

                var items = new List<SelectListItem>();
                List<CityModel> list = PropertyUtils.ConvertToList<CityModel>(CityBO.Instance.FindByAttribute("Inactive", 0));
                if (list.Count > 0)
                {
                    items = list.Select(p => new SelectListItem { Value = p.Name, Text = p.Name, Selected = false }).ToList();
                }
                if (defaultValue)
                    items.Insert(0, new SelectListItem { Text = textDefault, Value = string.Empty, Selected = true });

                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// Lấy tất cả danh sách VIP inactive cho dropdown
        /// </summary>
        /// <param name="defaultValue">Giá trị mặc định.</param>
        /// <param name="textDefault">Text thứ hai.</param>
        /// <returns>Danh sách VIP</returns>
        public static List<SelectListItem> GetVIPProvider(bool defaultValue = true, string textDefault = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textDefault)) textDefault = _textDefault + " VIP";

                var items = new List<SelectListItem>();
                List<VIPModel> list = PropertyUtils.ConvertToList<VIPModel>(VIPBO.Instance.FindByAttribute("Inactive", 0));
                if (list.Count > 0)
                {
                    items = list.Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.Name, Selected = false }).ToList();
                }
                if (defaultValue)
                    items.Insert(0, new SelectListItem { Text = textDefault, Value = string.Empty, Selected = true });

                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// Lấy tất cả danh sách MemberType inactive cho dropdown
        /// </summary>
        /// <param name="defaultValue">Giá trị mặc định.</param>
        /// <param name="textDefault">Text thứ hai.</param>
        /// <returns>Danh sách MemberType</returns>
        public static List<SelectListItem> GetMemberTypeProvider(bool defaultValue = true, string textDefault = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textDefault)) textDefault = _textDefault + " MemberType";

                var items = new List<SelectListItem>();
                List<MemberTypeModel> list = PropertyUtils.ConvertToList<MemberTypeModel>(MemberTypeBO.Instance.FindByAttribute("Inactive", 0));
                if (list.Count > 0)
                {
                    items = list.Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.Name, Selected = false }).ToList();
                }
                if (defaultValue)
                    items.Insert(0, new SelectListItem { Text = textDefault, Value = string.Empty, Selected = true });

                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// Lấy tất cả danh sách Profile là Agent cho dropdown
        /// </summary>
        /// <param name="defaultValue">Giá trị mặc định.</param>
        /// <param name="textDefault">Text thứ hai.</param>
        /// <returns>Danh sách Profile là Agent</returns>
        public static List<SelectListItem> GetProfileAgentProvider(bool defaultValue = true, string textDefault = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textDefault)) textDefault = _textDefault + " Agent";

                var items = new List<SelectListItem>();
                List<ProfileModel> list = PropertyUtils.ConvertToList<ProfileModel>(ProfileBO.Instance.FindByAttribute("Type", 1));
                if (list.Count > 0)
                {
                    items = list.Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.Account, Selected = false }).ToList();
                }
                if (defaultValue)
                    items.Insert(0, new SelectListItem { Text = textDefault, Value = string.Empty, Selected = true });

                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// Lấy tất cả danh sách Profile là Company cho dropdown
        /// </summary>
        /// <param name="defaultValue">Giá trị mặc định.</param>
        /// <param name="textDefault">Text thứ hai.</param>
        /// <returns>Danh sách Profile là Company</returns>
        public static List<SelectListItem> GetProfileCompanyProvider(bool defaultValue = true, string textDefault = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textDefault)) textDefault = _textDefault + " Company";

                var items = new List<SelectListItem>();
                List<ProfileModel> list = PropertyUtils.ConvertToList<ProfileModel>(ProfileBO.Instance.FindByAttribute("Type", 2));
                if (list.Count > 0)
                {
                    items = list.Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.Account, Selected = false }).ToList();
                }
                if (defaultValue)
                    items.Insert(0, new SelectListItem { Text = textDefault, Value = string.Empty, Selected = true });

                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// Lấy tất cả danh sách Profile là Contact cho dropdown
        /// </summary>
        /// <param name="defaultValue">Giá trị mặc định.</param>
        /// <param name="textDefault">Text thứ hai.</param>
        /// <returns>Danh sách Profile là Contact</returns>
        public static List<SelectListItem> GetProfileContactProvider(bool defaultValue = true, string textDefault = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textDefault)) textDefault = _textDefault + " Contact";

                var items = new List<SelectListItem>();
                List<ProfileModel> list = PropertyUtils.ConvertToList<ProfileModel>(ProfileBO.Instance.FindByAttribute("Type", 5));
                if (list.Count > 0)
                {
                    items = list.Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.Account, Selected = false }).ToList();
                }
                if (defaultValue)
                    items.Insert(0, new SelectListItem { Text = textDefault, Value = string.Empty, Selected = true });

                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// Lấy tất cả danh sách RoomType cho dropdown
        /// </summary>
        /// <param name="defaultValue">Giá trị mặc định.</param>
        /// <param name="textDefault">Text thứ hai.</param>
        /// <returns>Danh sách RoomType</returns>
        public static List<SelectListItem> GetRoomTyeProvider(bool defaultValue = true, string textDefault = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textDefault)) textDefault = _textDefault + " RoomType";

                var items = new List<SelectListItem>();
                List<RoomTypeModel> list = PropertyUtils.ConvertToList<RoomTypeModel>(RoomTypeBO.Instance.FindByAttribute("InActive", 0));
                if (list.Count > 0)
                {
                    items = list.Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.Code, Selected = false }).ToList();
                }
                if (defaultValue)
                    items.Insert(0, new SelectListItem { Text = textDefault, Value = string.Empty, Selected = true });

                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// Lấy tất cả danh sách Currency oomType cho dropdown
        /// </summary>
        /// <param name="defaultValue">Giá trị mặc định.</param>
        /// <param name="textDefault">Text thứ hai.</param>
        /// <returns>Danh sách Currency</returns>
        public static List<SelectListItem> GetCurrencyProvider(bool defaultValue = true, string textDefault = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textDefault)) textDefault = _textDefault + " RoomType";

                var items = new List<SelectListItem>();
                List<CurrencyModel> list = PropertyUtils.ConvertToList<CurrencyModel>(CurrencyBO.Instance.FindByAttribute("Inactive", 0));
                if (list.Count > 0)
                {
                    items = list.Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.Description, Selected = false }).ToList();
                }
                if (defaultValue)
                    items.Insert(0, new SelectListItem { Text = textDefault, Value = string.Empty, Selected = true });

                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<SelectListItem>();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using WxAppService.DbContext;
using WxModel;
using WxModel.DtoModel;

namespace WxAppService
{
    public class VoteCountAppService:SqlRepository<VoteCount>
    {

        /// <summary>
        /// 0 今天已经投票了，
        /// 1：未投票，投票成功
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public int IsVotedToday(int deptId, string openId)
        {
            var dt = DateTime.Now.ToShortDateString();

            var dtbeginStr = dt + " 00:00:00";
            var dtendStr = dt + " 23:59:59";
            
            var dtbegin =DateTime.Parse(dtbeginStr);
            var dtend = DateTime.Parse(dtendStr);
            var query = _dbClient.Queryable<VoteCount>().Where(s=>s.OpenId==openId &&s.InsertDate>dtbegin && s.InsertDate<=dtend).FirstOrDefault();
            if (query != null)
            {
                return 0;
            }
            else
            {
                var model=new VoteCount();
                model.DeptId = deptId;
                model.OpenId = openId;
                model.InsertDate=DateTime.Now;
                _dbClient.Insert(model);
            }
            return 1;
        }

        /// <summary>
        /// 门店投票排行
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<VoteCountDto> GetStoreVoteCountByDeptId(int id)
        {
            //获取所有投票的数量
            //int[] idsArray = new[] {1};
            //片区id
            var partArea = _dbClient.Queryable<Department>().Where(s => s.ParentId == id).ToList();
            var areaIdArray = partArea.Select(s => s.Id).ToArray();

            var voteCountList = _dbClient.Queryable<VoteCount>()
                            .JoinTable<Department>((v,s) => v.DeptId == s.Id)
                             .JoinTable<Department, Department>((v,s,p) => s.ParentId == p.Id)
                         .Where<Department>((v, s) => areaIdArray.Contains(s.ParentId)).
                 GroupBy("DeptId,s.Name,p.Name").Select<VoteCountDto>("v.DeptId,count(v.Id) VCount,s.Name DeptName,p.Name PartName").ToList();


            return voteCountList;

        }


        /// <summary>
        /// 房网和大众职能部门的投票排行
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<VoteCountDto> GetVoteCountByDeptId(int id)
        {
            //获取所有投票的数量
            //int[] idsArray = new[] {1};
          
               var voteCountList = _dbClient.Queryable<VoteCount>()
                               .JoinTable<Department>((v,s) => v.DeptId == s.Id)
                            .Where<Department>((v,s) =>s.ParentId==id).
                    GroupBy("DeptId,Name").Select<VoteCountDto>("v.DeptId,count(v.Id) VCount,s.Name DeptName").ToList();

           
            return voteCountList;

        }
    }
}

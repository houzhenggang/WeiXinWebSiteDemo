using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SqlSugar;
using WxAppService.DbContext;
using WxModel;
using WxModel.Base;
using WxModel.DtoModel;

namespace WxAppService
{
    public class DeptVoteAppService:SqlRepository<Department>
    {
        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="dto"></param>
        public void AddDepartment(VoteDeptDto dto)
        {
            var model=new Department();
            model.DeptSummary = dto.DeptSummary;
            model.DeptType = dto.DeptType;
            model.Name = dto.DelptName;
            model.ParentId = dto.ParentId;
            model.InsertDate=DateTime.Now;

            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (_dbClient)
                    {
                        var id = _dbClient.Insert(model);

                        if (!string.IsNullOrEmpty(dto.ImgIds))
                        {
                            var imgIdArray = dto.ImgIds.Split(',');

                            if (imgIdArray.Length > 0)
                            {
                                int intId = int.Parse(id.ToString());
                                int[] intArray = Array.ConvertAll<string, int>(imgIdArray, delegate(string s) { return int.Parse(s); });

                                _dbClient.Update<DeptImg>(new { ParentId = intId }, it => intArray.Contains(it.Id));
                            }
                        }
                    }
                   
                    transaction.Complete();
                }
             
               // _dbClient.CommitTran();
            }
            catch (Exception ex)
            {
                //_dbClient.RollbackTran();
                throw ex;
            }

            // return returnModel.Id;

            //var id = _dbClient.Insert(model);
         
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        public void DeleteDeptData(int id)
        {
            using (var transaction = new TransactionScope())
            {
                using (_dbClient)
                {
                    _dbClient.Delete<Department>(s => s.Id == id);
                    _dbClient.Delete<DeptImg>(s => s.ParentId == id);
                }

                transaction.Complete();
            }
        }

        /// <summary>
        /// 跟新数据
        /// </summary>
        /// <param name="dto"></param>
        public void UpdateDeptDataById(VoteDeptDto dto)
        {
            var model = new Department();
            model.Id = dto.Id;
            model.DeptSummary = dto.DeptSummary;
            model.DeptType = dto.DeptType;
            model.Name = dto.DelptName;
            model.ParentId = dto.ParentId;
            model.InsertDate = DateTime.Now;

            using (var transaction = new TransactionScope())
            {
                using (_dbClient)
                {

                    _dbClient.Update<Department>(model);

                    if (!string.IsNullOrEmpty(dto.ImgIds))
                    {
                        var imgIdArray = dto.ImgIds.Split(',');

                        if (imgIdArray.Length > 0)
                        {
                          
                            int[] intArray = Array.ConvertAll<string, int>(imgIdArray, delegate (string s) { return int.Parse(s); });

                            _dbClient.Update<DeptImg>(new { ParentId = dto.Id }, it => intArray.Contains(it.Id));
                        }
                    }
                }

                transaction.Complete();
            }

        }
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VoteDeptDto GetDeptDataById(int id)
        {
            var dto=new VoteDeptDto();

            var query = _dbClient.Queryable<Department>().Where(s => s.Id == id)
                .Select<VoteDeptDto>(dept=>new VoteDeptDto()
                {
                    Id = dept.Id,
                    ParentId = dept.ParentId,
                    DelptName = dept.Name,
                    DeptType = dept.DeptType,
                    DeptSummary = dept.DeptSummary
                }).ToList();
            if (query.Any())
            {
                var deptImg = _dbClient.Queryable<DeptImg>().Where(s => s.ParentId == id).ToList();
                var deptImgfc = deptImg.Where(s => s.ImgType == 0).ToList();
                var deptImgcover= deptImg.Where(s => s.ImgType ==1).ToList();
                dto = query[0];
                dto.DeptImgs = deptImg;
            }

            return dto;
        }

        /// <summary>
        /// 根据pid获取下级部门
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public List<VoteDeptDto> GetDeptTitleByPid(int pid)
        {
            //var dto = new List<VoteDeptDto>();

            var query = _dbClient.Queryable<Department>().Where(s => s.ParentId == pid)
                .Select<VoteDeptDto>(dept => new VoteDeptDto()
                {
                    Id = dept.Id,
                    ParentId = dept.ParentId,
                    DelptName = dept.Name,
                    DeptType = dept.DeptType,
                    DeptSummary = dept.DeptSummary
                }).ToList();

            //获取所有部门的封面图片
            var idsArray = query.Select(s => s.Id).ToArray();
            var deptImgs = _dbClient.Queryable<DeptImg>().Where(s => idsArray.Contains(s.ParentId) && s.ImgType == 1).ToList();
            if (query.Any())
            {
                query.ForEach(m=>
                {
                    var deptImg = deptImgs.Where(s => s.ParentId ==m.Id).FirstOrDefault();

                    m.CoverImg = deptImg == null ? "" : deptImg.ImgUrl;
                });
            }

            return query;
        }
        /// <summary>
        /// 分页数据列表
        /// </summary>
        /// <param name="pIndex"></param>
        /// <param name="pSize"></param>
        /// <param name="deptType"></param>
        /// <returns></returns>
        public GridList<Department> GetDeptList(int pIndex, int pSize, int deptType)
        {
          var gridList=new GridList<Department>(); 
             int rows = 0, totalPage=0;
             var query = GetPageList(pIndex, pSize, out rows, out totalPage, s => s.DeptType == deptType, false,
                s => s.InsertDate);

            gridList.page = pIndex;
            gridList.size = pSize;
            gridList.records = rows;
            gridList.total = totalPage;
            gridList.rows = query;
            return gridList;
        }

        /// <summary>
        /// 返回列表
        /// </summary>
        /// <param name="deptType"></param>
        /// <returns></returns>
        public List<Department> GetDeptDic(int deptType)
        {

            var query = _dbClient.Queryable<Department>().Where(s => s.DeptType == deptType).ToList();

            return query;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public List<Department> GetDeptByPid(int pId)
        {
            var query = _dbClient.Queryable<Department>().Where(s => s.ParentId == pId).ToList();

            return query;
        }

        /// <summary>
        /// 获取相关投票列表数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VoteDetailDto GetVoteDetailByPartId(int id)
        {
            var model=new VoteDetailDto();

            var query = _dbClient.Queryable<Department>().Where(s => s.Id == id || s.ParentId == id).ToList();

            var partModel = query.Where(s => s.Id == id).FirstOrDefault();
            if (partModel != null)
            {
                model.PartName = partModel.Name;
                query.Remove(partModel);

                //获取所有部门的封面图片
                var idsArray = query.Select(s => s.Id).ToArray();
                var deptImgs= _dbClient.Queryable<DeptImg>().Where(s=>idsArray.Contains(s.ParentId) &&s.ImgType == 1).ToList();

                //获取所有投票的数量

                var voteCountList = _dbClient.Queryable<VoteCount>().Where(c =>idsArray.Contains(c.DeptId)).
                        GroupBy(it => it.DeptId).Select<VoteCountDto>("DeptId,count(Id) VCount").ToList();

                if (query.Any())
                {
                    query.ForEach(m =>
                    {
                        //int voteCount = _dbClient.Queryable<VoteCount>().Where(s => s.DeptId == m.Id).Count();
                        var voteCountModel = voteCountList.Where(s => s.DeptId == m.Id).FirstOrDefault();
                        var detailModel=new VoteDetailSonDto();
                        detailModel.DeptId = m.Id;
                        detailModel.DeptName = m.Name;
                        detailModel.VoteCount = voteCountModel==null?0:voteCountModel.VCount;

                        var deptImg = deptImgs.Where(s => s.ParentId == m.Id).FirstOrDefault();

                        detailModel.CoverImg = deptImg == null ? "" : deptImg.ImgUrl;
                        model.Detial.Add(detailModel);
                    });
                }

            }

            return model;
        }

        /// <summary>
        ///获取大众房产和房网的封面图片
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<DeptCoverDto> GetDeptCover(int[] ids)
        {
            var list=new List<DeptCoverDto>();

            list =
                _dbClient.Queryable<DeptImg>()
                    .Where(s => ids.Contains(s.ParentId) && s.ImgType == 1)
                    .Select<DeptCoverDto>("top 1 ParentId DeptId,ImgUrl CoverImg").ToList();
            return list;
        } 
    }
}

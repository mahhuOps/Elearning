using Core.Implements;
using Core.Models;
using DatabaseService.Enums;
using DatabaseService.Interfaces;
using Elearning.BL.Interfaces;
using Elearning.Models;
using System.Data;

namespace Elearning.BL.Implements
{
    public class PromotionBL : BaseBL<Promotion>, IPromotionBL
    {
        public PromotionBL(IDatabaseService databaseService) : base(databaseService)
        {
        }

        public override List<ValidateResult> ValidateBeforeSave<Entity>(Entity entity)
        {
            var validateResults = base.ValidateBeforeSave(entity);
            var promotion = (entity as  Promotion);

            if(promotion?.State != ModelStateEnum.Insert && promotion?.State != ModelStateEnum.Update) return validateResults;
            if(promotion?.PromotionType == Common.Enum.PromotionType.Percent && promotion.PromotionValue > 100)
                validateResults.Add(new ValidateResult {
                    ErrorCode = Core.Enums.ErrorCodeEnum.ValueInvalid,
                    FieldError = "PromotionValue",
                    FieldValue = promotion.PromotionValue
                });

            if(promotion?.ExpireDate != null && promotion.ExpireDate < DateTime.Now)
                validateResults.Add(new ValidateResult
                {
                    ErrorCode = Core.Enums.ErrorCodeEnum.ValueInvalid,
                    FieldError = "ExpireDate",
                    FieldValue = promotion.ExpireDate
                });

            return validateResults;
        }

        public override void AfterSave<Entity>(Entity entity, IDbConnection connection, IDbTransaction transaction)
        {
            base.AfterSave(entity, connection, transaction);
            var promotion = (entity as Promotion);

            if ((promotion?.State == ModelStateEnum.Insert || promotion?.State == ModelStateEnum.Update) && promotion.IsActive == true)
            {
                var sql = "UPDATE promotion SET IsActive = false WHERE PromotionID <> @PromotionID";

                _dataBaseService.ExecuteUsingCommandText(sql, new Dictionary<string, object>
                {
                    {"@PromotionID", promotion?.PromotionID ?? 0 }
                }, transaction,connection);
            }
        }
    }
}

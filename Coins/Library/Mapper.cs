using Coins.Repositories;
using System;
using System.Reflection;

namespace Coins.Library
{
    public class Mapper<TEntity, TViewModel> where TEntity : class
    {
        public TEntity ViewModelToEntity(TEntity entity, TViewModel viewModel)
        {
            foreach (var item in viewModel.GetType().GetProperties())
            {
                var entityProperty = entity.GetType().GetProperty(item.Name);
                try
                {
                    entityProperty.SetValue(
                        entity,
                        Convert.ChangeType(item.GetValue(viewModel, null), entityProperty.PropertyType),
                        null
                    );
                }
                catch { }
            }

            return entity;
        }

        public TViewModel EntityToViewModel(TViewModel viewModel, TEntity entity)
        {
            foreach (var item in entity.GetType().GetProperties())
            {
                var viewModelProperty = viewModel.GetType().GetProperty(item.Name);

                try
                {
                    viewModelProperty.SetValue(
                        viewModel,
                        Convert.ChangeType(item.GetValue(entity, null), viewModelProperty.PropertyType),
                        null
                    );
                }
                catch { }
            }

            return viewModel;
        }
    }
}

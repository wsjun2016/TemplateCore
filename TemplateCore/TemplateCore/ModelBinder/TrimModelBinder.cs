using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BasisFrameWork.Extension;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using TemplateCore.Attributes;

namespace TemplateCore.ModelBinder {
    /// <summary> 
    /// 在进行模型绑定的时候，去除string类型参数的首位空白字符。
    /// 适用于简单的模型绑定，如FromQuery/FromForm等。
    /// 作用在属性上，如：
    ///  [ModelBinder(typeof(TrimModelBinder))] public string Data{get;set;}
    /// </summary>
    public class TrimModelBinder : IModelBinder {
        public Task BindModelAsync(ModelBindingContext bindingContext) {
            //1.如果当前绑定的对象不是string类型的，则返回
            if (bindingContext.ModelMetadata.ModelType != typeof(string))
                return Task.CompletedTask;

            //2.找到被绑定属性的自定义的特性(Attrbute)集合
            //找到元数据的属性
            var properties = bindingContext.ModelMetadata?.GetType()?.GetProperties();
            //找到名称为Attributes的属性
            var attributesProperty = properties?.First(it => it.Name == "Attributes");
            //获取名称为Attributes属性的值
            var values = (ModelAttributes)attributesProperty?.GetValue(bindingContext.ModelMetadata);
            List<Attribute> attributes = values?.Attributes.Cast<Attribute>().ToList();

            //3.找到被绑定对象是否存在NoTrimAttribute特性
            bool isNoTrim = attributes?.Any(it => it.GetType().IsAssignableFrom(typeof(NoTrimAttribute))) ?? false;

            //如果Body方式，即ContentType=application/json，则需要通过序列化Request.Body去绑定数据，此时ValueProvider为null
            //如果Form/QueryString/Route等提交的，则可以用ValueProvider获取客户端传入的值
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            //如果没有获取到值，则返回
            if (value == ValueProviderResult.None)
                return Task.CompletedTask;
            //格式化最终结果
            var result = isNoTrim ? value.FirstValue : value.FirstValue.Trim();

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, new ValueProviderResult(result.Trim()));
            bindingContext.Result = ModelBindingResult.Success(result);

            return Task.CompletedTask;
        }
    }
}

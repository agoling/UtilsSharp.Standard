**详细使用说明文档**：https://agoling.github.io/UtilsSharpDoc/#/

# UtilsSharp帮助类

> 这篇文章是《UtilsSharp .NET框架》专辑的**第1篇**文章，使用的`.NET Standard`版本为`2.0`。
> `.NET Standard2.0` 版本支持`.NET Framework 4.6.1`及以上版本、`.NET core 2.0`及以上版本、`Mono5.4`及以上版本、`Xamarin.iOS10.14`及以上版本、`Xamarin.Mac3.8`及以上版本、`Xamarin.Android8.0`及以上版本、通用 Windows 平台10.0.16299及以上版本，UtilsSharp.Standard工具帮助类库是基于`.NET standard2.0`封装的，里面包括：出入参规范类、钉钉机器人帮助类、图片帮助类、文件帮助类、随机数帮助类、对象映射帮助类、字符串帮助类、验证码生成帮助类、时间帮助类、中国日历帮助类、任务下发帮助类、正则帮助类等，后面将持续更新中…
>
> **可以在`Nuget` 搜索`UtilsSharp`包并安装使用**

####  一、命名规范

##### 1、返回参小驼峰命名法

除第一个单词之外，其他单词首字母大写，例如：`getKeyword`，`getOccResponse`

##### 2、方法命名规范

获取单个对象的方法用`Get`作为前缀，如：`GetRecord`

获取多个对象的方法用`Search`作为前缀，如：`SearchRecord`

添加的方法用`Add`作为前缀，如：`AddRecord`

修改的方法用`Modify`作为前缀，如：`ModifyRecord`

(添加+修改)的方法用`Save`作为前缀，如：`SaveRecord`

删除的方法用`Delete`作为前缀，如：`DeleteRecord`

清空的方法用`Clear`作为前缀，如：`ClearRecord`

判断类的方法用`Is`作为前缀，如：`IsRegister`,`IsExist`

##### 3、变量命名规范

变量取名应见名识意,尽量用英文单词,而不是缩写

例如：`PageIndex`, `PageSize`

##### 4、入参、出参类名统一，入参+Request，出参+Response

例如：

```c#
/// <summary>
/// 会员登录请求参数
/// </summary>
public class LoginRequest
{
   /// <summary>
   /// 用户名
   /// </summary>
   public string UserName { set; get; }
   /// <summary>
   /// 密码
   /// </summary>
   public string UserPsw { set; get; }

}
/// <summary>
/// 会员登录返回参数
/// </summary>
public class LoginResponse
{
   /// <summary>
   /// 访问口令
   /// </summary>
   public string Token { set; get; }

   /// <summary>
   /// 用户名
   /// </summary>
   public string UserName { set; get; }
}
```

#### 二、请求接口返回格式：BaseResut&lt;T&gt;

##### 1、请求方法例子：

```c#
/// <summary>
/// 登入
/// </summary>
/// <param name="request">登入参数</param>
/// <returns></returns>
public BaseResult<LoginResponse> Login(LoginRequest request)
{
   var result = new BaseResult<LoginResponse>();
   try
   {
      if (string.IsNullOrEmpty(request.UserName))
      {
          result.SetError("用户名不能为空", BaseStateCode.参数不能为空);
          return result;
       }
       if (string.IsNullOrEmpty(request.UserPsw))
       {
          result.SetError("密码不能为空", BaseStateCode.参数不能为空);
          return result;
       }
       /*…这边省略登入业务代码…*/
       var response = new LoginResponse {UserName = request.UserName, Token = Guid.NewGuid().ToString("N")};
       result.SetOkResult(response,"登入成功");
       return result;
    }
    catch (Exception ex)
    {
      var errorCode = Guid.NewGuid().ToString("N");
      //这边用errorCode作为日志主键，把错误信息写入到日志
      var errorMessage = errorCode.ToMsgException();
      result.SetError(errorMessage, BaseStateCode.TryCatch异常错误);
      return result;
    }
}
```

##### 2、请求返回的成功结构：

```json
{
    "result": {
        "token": "ecbcc910fab0497c933d69906188ad0e",
        "userName": "xxx"
    },
    "code": 200,
    "msg": "登入成功"
}
```

#### 三、请求列表接口基础出入参,入参：BasePage，有排序的入参：BaseSortPage，出参：BasePagedResult&lt;T&gt;

##### 1、分页请求的基础参数

```c#
{
 keyword:"", //关键词搜索
 pageIndex:1, //当前页码
 pageSize:10, //分页数量
}
```

##### 2、请求方法例子：

```c#
/// <summary>
/// 分页搜索
/// </summary>
/// <param name="request">登入参数</param>
/// <returns></returns>
public BasePagedResult<LoginResponse> Search(BaseSortPage request)
{
   var result = new BasePagedResult<LoginResponse>();
   try
   {
      var response = new BasePagedInfoResult<LoginResponse>()
      {
          PageIndex = request.PageIndex,
          PageSize = request.PageSize
      };
      if (string.IsNullOrEmpty(request.Keyword))
      {
          result.SetError("搜索关键词不能为空！",BaseStateCode.参数不能为空);
          return result;
      }
          /*…这边省略查询业务代码…*/
          /*…以下模拟从数据库获取数据…*/
      response.List = new List<LoginResponse>
      {
         new LoginResponse() {Token = Guid.NewGuid().ToString(), UserName = "xxx1"},
         new LoginResponse() {Token = Guid.NewGuid().ToString(), UserName = "xxx2"},
         new LoginResponse() {Token = Guid.NewGuid().ToString(), UserName = "xxx3"}
      };
      response.PageIndex = request.PageIndex;
      response.PageSize = request.PageSize;
      response.TotalCount = 100;
      response.Params = request;
      result.SetOkResult(response);
      return result;
    }
    catch (Exception ex)
    {
      var errorCode = Guid.NewGuid().ToString("N");
      //这边用errorCode作为日志主键，把错误信息写入到日志
      var errorMessage = errorCode.ToMsgException();
      result.SetError(errorMessage, BaseStateCode.TryCatch异常错误);
      return result;
    }
}
```

##### 3、分页请求返回的成功结构：

```json
{
    "result": {
        "pageIndex": 1,//当前页码
        "pageSize": 10,//分页数量
        "totalCount": 100,//总条数
        "totalPages": 10,//总页数 
        "orderBy": null,//排序
        "hasPreviousPage": false,//是否有上一页
        "hasNextPage": true,//是否有下一页
        "list": [    //返回的列表数据
            {
                "token": "411349e5-d345-4bfa-8022-d109fc483aeb",
                "userName": "xxx1"
            },
            {
                "token": "3a72e1d0-73dc-48d8-972b-7dde831101c3",
                "userName": "xxx2"
            },
            {
                "token": "9d690366-cc61-4ca5-9f08-f23972ebe27a",
                "userName": "xxx3"
            }
        ],
        "params": {
            "sortField": null,
            "sortType": null,
            "pageIndex": 1,
            "pageSize": 10,
            "keyword": "连衣裙"
        }
    },
    "code": 200,
    "msg": "请求成功"
}
```

#### 四、错误码

| 返回码 | 标识          | 说明           |
| ------ | ------------- | -------------- |
| 200    | success       | 请求成功       |
| 999    | defaultTips   | 业务提示       |
| 2000   | apiError      | 接口异常       |
| 3000   | networkError  | 网络异常       |
| 4000   | notLogin      | 未登录         |
| 4010   | authExpire    | 授权到期       |
| 5000   | exception     | 异常错误       |
| 6000   | dataNotFound  | 数据找不到     |
| 6010   | dataNotValid  | 数据验证不通过 |
| 7000   | businessError | 默认业务性异常 |
| 8000   | dbError       | 数据库异常     |
| 9000   | SystemError   | 系统错误       |

**详细使用说明文档**：https://agoling.github.io/UtilsSharpDoc/#/

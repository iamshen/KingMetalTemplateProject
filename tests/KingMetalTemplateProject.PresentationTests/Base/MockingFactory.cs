using System.Text;

namespace KingMetalTemplateProject.PresentationTests;

/// <summary>
///     随机生成类
/// </summary>
public static class MockingFactory
{
    private static readonly Random Random = new();

    /// <summary>
    ///     随机中文
    /// </summary>
    /// <returns></returns>
    public static string ChineseText(int length = 100)
    {
        var chineseChars =
            "的一是在不了有和人这中大为上个国我以要他时来用们生到作地于出就分对成会可主发年动同工也能下过子说产种面而方后多定行学法所民得经十三之进着等部度家电力里如水化高自二理起小物现实加量都两体制机当使点从业本去把性好应开它合还因由其些然前外天政四日那社义事平形相全表间样与关各重新线内数正心反你明看原又么利比或但质气第向道命此变条只没结解问意建月公无系军很情者最立代想已通并提直题党程展五果料象员革位入常文总次品式活设及管特件长求老头基资边流路级少图山统接知较将组见计别她手角期根论运农指几九区强放决西被干做必战先回则任取据处队南给色光门即保治北造百规热领七海口东导器压志世金增争济阶油思术极交受联什认六共权收证改清己美再采转更单风切打白教速花带安场身车例真务具万每目至达走积示议声报斗完类八离华名确才科张信马节话米整空元况今集温传土许步群广石记需段研界拉林律叫且究观越织装影算低持音众书布复容儿须际商非验连断深难近矿千周委素技备半办青省列习响约支般史感劳便团往酸历市克何除消构府称太准精值号率族维划选标写存候毛亲快效斯院查江型眼王按格养易置派层片始却专状育厂京识适属圆包火住调满县局照参红细引听该铁价严龙飞推存运确争陈志读降食卫角色察洋热稳零杂讲任连项布市半片史引听该铁价严龙飞推存运确争陈志读降食卫角色察洋热稳零杂讲任连项布市。";

        var sb = new StringBuilder();
        for (var i = 0; i < length; i++) sb.Append(chineseChars[Random.Next(chineseChars.Length)]);

        return sb.ToString();
    }
}
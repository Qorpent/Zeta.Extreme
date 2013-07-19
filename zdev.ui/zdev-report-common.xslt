<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:param name="selfname" >default</xsl:param>
  <xsl:param name="docroot" >attr</xsl:param>
  <xsl:param name="title">УКАЖИТЕ ИМЯ ОТЧЕТА</xsl:param>


  <xsl:template match="result">
    <html>
      <head>
        <title>
          <xsl:value-of select="$title"/>
        </title>
		<script src="../scripts/jquery.min.js" type="text/javascript">&#160;</script>
        <meta generator="qorpent:xslt-render(${selfname}.xslt)" />
        <link rel="stylesheet" href="../styles/{$selfname}.css" />
      </head>
      <body>
        <div class="header">Разработчик Zeta v0.2</div>
        <h1>
          <xsl:value-of select="$title"/>
        </h1>
        <xsl:apply-templates select="." mode="body"/>
      </body>
    </html>

  </xsl:template>

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>

  <xsl:template name="attr-stat-table">
    <xsl:param name="class"></xsl:param>
    <table class="data attribute stats {$class}">
      <thead>
        <tr>
          <th>№</th>
          <th>Метки</th>
          <th>Код атрибута</th>
          <th>Информация</th>
          <th>Число значений</th>
          <th>Число ссылок</th>
        </tr>
      </thead>
      <tbody>
        <xsl:apply-templates select="item"  mode="attr-stats">
          <xsl:sort select="@Name"/>
        </xsl:apply-templates>
      </tbody>
    </table>
  </xsl:template>

  <xsl:template match="result/item" mode="attr-stats">
    <tr class="obsolete-{Doc/@IsObsolete} error-{Doc/@IsError} question-{Doc/@IsQuestion}">
      <td class="number">
        <xsl:number value="position()" />
      </td>
      <td class="marks">
        <xsl:apply-templates select="Doc" mode="marks"/>
      </td>
      <td class="code">
        <xsl:choose>
          <xsl:when test="ValueVariants/item">
            <a href="#attr-detail-{@Name}">
              <xsl:value-of select="@Name"/>
            </a>

          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="@Name"/>
          </xsl:otherwise>
        </xsl:choose>
      </td>
      <td class="doc">
        <xsl:apply-templates select="Doc" mode="in-cell-short"/>

      </td>
      <td class="varcount">
        <xsl:value-of select="@VariantCount"/>
      </td>
      <td class="refcount">
        <xsl:value-of select="@ReferenceCount"/>
      </td>
    </tr>
  </xsl:template>
  <xsl:template match="result/item" mode="attr-detail">
    <h3>
      <a name="attr-detail-{@Name}"/> Атрибут <xsl:value-of select="@Name"/>
    </h3>
    <div>
      <a href="#toc">назад к оглавлению</a>
    </div>
	<br/>
    <xsl:apply-templates select="Doc" mode="in-text-full"/>
	<br/>
    <xsl:call-template name="attr-variant-stat-table"/>
  </xsl:template>
  <xsl:template match="Doc"  mode="in-cell-short">
    <ul class="doc">
      <li>
        <xsl:value-of select="@Name"/>
      </li>
      <xsl:if test="string(@Comment)">
        <li>
          <xsl:value-of select="@Comment" disable-output-escaping="yes"/>
        </li>
      </xsl:if>
	   <xsl:if test="string(@Question)">
        <li class="question-{@IsQuestion}">
          <xsl:value-of select="@Question"/>
        </li>
      </xsl:if>
      <xsl:if test="string(@Obsolete)">
        <li class="obsolete-{@IsObsolete}">
          <xsl:value-of select="@Obsolete"/>
        </li>
      </xsl:if>
	   <xsl:if test="string(@Error)">
        <li class="error-{@IsError}">
          <xsl:value-of select="@Error"/>
        </li>
      </xsl:if>
    </ul>

  </xsl:template>
  <xsl:template match="Doc"  mode="marks">
    <xsl:if test="@IsBiztran">
      <span class="mark biztran">BIZTRAN</span>
    </xsl:if>
  </xsl:template>
  <xsl:template match="Doc"  mode="in-text-full">
    <table class="data doc">
      <tbody>
        <tr>
          <td>Краткое описание</td>
          <td>
            <xsl:value-of select="@Name"/>
          </td>
        </tr>
        <xsl:if test="string(@Comment)">
          <tr>
            <td>Разъяснение</td>
            <td>
              <xsl:value-of select="@Comment" disable-output-escaping="yes"/>
            </td>
          </tr>
        </xsl:if>
		 <xsl:if test="string(@Question)">
          <tr class="question-{@IsQuestion}">
            <td>Вопросы</td>
            <td>
              <xsl:value-of select="@Question"/>
            </td>
          </tr>
        </xsl:if>
		
        <xsl:if test="string(@Obsolete)">
          <tr class="obsolete-{@IsObsolete}">
            <td>Устаревший</td>
            <td>
              <xsl:value-of select="@Obsolete"/>
            </td>
          </tr>
        </xsl:if>
		
		 <xsl:if test="string(@Error)">
          <tr class="error-{@IsError}">
            <td>Ошибка</td>
            <td>
              <xsl:value-of select="@Error"/>
            </td>
          </tr>
        </xsl:if>
        <tr class="marks">
          <td>Метки</td>
          <td>
            <xsl:apply-templates select="." mode="marks"/>
          </td>
        </tr>

      </tbody>

    </table>

  </xsl:template>

  <xsl:template name="attr-variant-stat-table">
    <table class="data attr-variant stats">
      <thead>
        <th>№</th>
        <th>Значение</th>
        <th>Информация</th>
        <th>Число ссылок</th>
        <th>Ссылки</th>
      </thead>
      <tbody>
        <xsl:apply-templates select="ValueVariants/item" mode="attr-variant-stats">
          <xsl:sort select="@ReferenceCount" order="descending" data-type="number"/>
        </xsl:apply-templates>
      </tbody>
    </table>
  </xsl:template>

  <xsl:template match="ValueVariants/item" mode="attr-variant-stats">
    <tr  class="obsolete-{Doc/@IsObsolete} error-{Doc/@IsError} question-{Doc/@IsQuestion}">
      <td class="number">
        <xsl:number value="position()" />
      </td>
      <td class="value">
        <xsl:value-of select="@Value"/>
      </td>
      <td class="doc">
        <xsl:apply-templates select="Doc" mode="in-cell-short"/>

      </td>
      <td class="refcount">
        <xsl:value-of select="@ReferenceCount"/>
      </td>
	  
	  <xsl:choose>
			<xsl:when test="count(References//item) &gt; 4">
				 <td class="refs hidden" onclick="$(this).toggleClass('hidden')">
					<div class="ref-div">
						<xsl:apply-templates select="References" mode="ref-list"/>
					</div>
					<div class="ref-subst">
						Щелкните для просмотра
					</div>
				  </td>
			</xsl:when>
			<xsl:otherwise>
				<td class="refs">
					<xsl:apply-templates select="References" mode="ref-list"/>
				 </td>
			</xsl:otherwise>
	  </xsl:choose>
	  
    
    </tr>
  </xsl:template>

  <xsl:template match="References" mode="ref-list">
    <ul class="attr-ref l0" >
      <xsl:apply-templates select="item" mode="ref-list">
        <xsl:with-param name="level" select="1"/>
      </xsl:apply-templates>
    </ul>
  </xsl:template>

  <xsl:template match="References//item" mode="ref-list">
    <xsl:param name="level"/>
    <li>
      <xsl:apply-templates select="." mode="ref-desc"/>
      <xsl:if test=".//item">
        <ul  class="l{$level}">
          <xsl:apply-templates select="./item" mode="ref-list" >
            <xsl:with-param name="level" select="$level + 1"/>
          </xsl:apply-templates>
        </ul>
      </xsl:if>
    </li>
  </xsl:template>

  <xsl:template match="References//item" mode="ref-desc">
    <xsl:if test="@File">
      <xsl:value-of select="@File"/>
      <xsl:if test="@MainContext"> :: </xsl:if>
    </xsl:if>
    <xsl:if test="@MainContext">
      <xsl:value-of select="@MainContext"/>
      <xsl:if test="@SubContext"> :: </xsl:if>
    </xsl:if>
    <xsl:if test="@SubContext">
      <xsl:value-of select="@SubContext"/>
      <xsl:if test="@Line"> :: </xsl:if>
    </xsl:if>
    <xsl:if test="@Line">
      <xsl:value-of select="@Line"/>
    </xsl:if>
  </xsl:template>


  <xsl:template match="result" mode="body">
    <h2>
      <a name="toc"/> Общая статистика использования параметров
    </h2>
    <xsl:call-template name="attr-stat-table">
      <xsl:with-param name="class">param</xsl:with-param>
    </xsl:call-template>
    <h2>Характеристика отдельных атрибутов</h2>
    <p>Внимание: в характеристику включены только атрибуты с описанием отдельных значений</p>
    <xsl:apply-templates select="item[ValueVariants/item]" mode="attr-detail">
      <xsl:sort select="@Name"/>
    </xsl:apply-templates>
  </xsl:template>

</xsl:stylesheet>

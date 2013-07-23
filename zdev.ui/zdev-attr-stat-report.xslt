<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:import href="zdev-report-common.xslt"/>
  
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
      <xsl:apply-templates select="References" mode="collapsed-cell"/>




    </tr>
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

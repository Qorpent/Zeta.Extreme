<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  
  
  
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
	   <xsl:if test="string(@SubComment)">
        <li class="subcomment-{@IsSubComment}">
          <xsl:value-of select="@SubComment"/>
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
	<xsl:if test="@IsSystem">
      <span class="mark system">SYSTEM</span>
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
		<xsl:if test="string(@SubComment)">
          <tr>
            <td>Доп. комментарий</td>
            <td>
              <xsl:value-of select="@SubComment" disable-output-escaping="yes"/>
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
	   <xsl:if test="string(@SubComment)">
        <li class="subcomment-{@IsSubComment}">
          <xsl:value-of select="@SubComment"/>
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
	<xsl:if test="@IsSystem">
      <span class="mark system">SYSTEM</span>
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
		<xsl:if test="string(@SubComment)">
          <tr>
            <td>Доп. комментарий</td>
            <td>
              <xsl:value-of select="@SubComment" disable-output-escaping="yes"/>
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
</xsl:stylesheet>

<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>

  <xsl:template match="/">
    <html>
      <head>
        <title>Отчет о соответствии настроек администраторов</title>
        <style>
          table {
            border-collapse:collapse;
          }
          table td, table th {
            border : solid 1px gray;
          }
        </style>
      </head>
      <body>
        <h1>Отчет о соответствии настроек администраторов</h1>
        <table>
          <thead>
            <th>Номер</th>
            <th>Статус</th>
            <th>Предприятие</th>
            <th>Администратор(ы)</th>
          </thead>
          <tbody>
            <xsl:apply-templates select="//root/result/item"/>
          </tbody>
        </table>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="item">
    <tr class="fa-{@FileAttached} cnt-{@ActiveAdminCount} v-{@IsValid}">
      <td>
        <xsl:value-of select="position()"/>
      </td>
      <td>
        <ul>
          <xsl:if test="@IsValid = 'true'">
            <li>ОК</li>
          </xsl:if>
          <xsl:if test="@FileAttached = 'false'">
            <li>Нет файла</li>
          </xsl:if>
          <xsl:if test="@ActiveAdminCount = 0">
            <li>Админ не назначен</li>
          </xsl:if>
          <xsl:if test="@ActiveAdminCount &gt; 1">
            <li>Несколько администраторов</li>
          </xsl:if>
        </ul>
      </td>
      <td>
        <xsl:value-of select="@ObjectName"/>
      </td>
      <td>
        <ul>
         <xsl:apply-templates select="Admins/item"/>
        </ul>
      </td>
    </tr>
  </xsl:template>
  <xsl:template match="Admins/item">
    <li>
      (<xsl:value-of select="@Login"/>) <xsl:value-of select="@Name"/> ( <xsl:value-of select="@ObjectName"/> )
    </li>
  </xsl:template>
</xsl:stylesheet>

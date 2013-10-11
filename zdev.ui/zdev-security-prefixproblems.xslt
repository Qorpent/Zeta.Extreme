<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" indent="yes"/>

    <xsl:template match="@* | node()">
        <xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
        </xsl:copy>
    </xsl:template>
  <xsl:template match="/">
  <html>
    <head>
      <title>Отчет о проблемах в настройке безопасности</title>
      <style>
        table {
        border-collapse:collapse;
        margin-top:10px;
        
        }
        table td, table th {
        border : solid 1px gray;
        padding:2px;
        }
        table.problem {
        width:200px;
        margin-top:0px;
        }
        td.problem{
          padding:0px;
        }
        td.prefix{
        width:100px;
        }
        td.number{
        width:50px;
        }

      </style>
    </head>
    <body>
      <h1>Отчет о проблемах в настройке безопасности</h1>
      <p>В колонке "Проблемы" указаны Роли по которым есть превышение количества Операторов и/или Подписантов.</p>
      <p>Параметры отчета</p>
        
          <xsl:apply-templates select="//root/result/Parameters"/>
     
      <table>
        <thead>
          <th>Номер</th>
          <th>ID</th>
          <th>Предприятие</th>
          <th>Проблемы </th>
        </thead>
        <tbody>
          <xsl:apply-templates select="//root/result/ProblemPrefixObjects/item"/>
        </tbody>
      </table>
    </body>
  </html>
</xsl:template>
  
  <xsl:template match="Parameters">
  
    <table>
       
      <tr>
          <td>Неустановленные префиксы - проблема</td>
      <td>
        
        <xsl:value-of select="@NotSetPrefixIsProblem"/>
      </td>
          </tr>
      <tr>
          <td>Осутсвие хотя бы одного Подписанта - проблема</td>
      <td>
        <xsl:value-of select="@NotSetUnderwriterIsProblem"/>
      </td>
        </tr>
      <tr>
           <td>Количество Опреаторов больше 5 (по умолчанию) - проблема</td>
      <td>
        <xsl:value-of select="@MaxOperators"/>
      </td>
        </tr>
      <tr>
          <td>Количество Подписантов больше 5 (по умолчанию) - проблема</td>
      <td>
        <xsl:value-of select="@MaxUnderwriters"/>
      </td>
        </tr>
      <tr>
           <td>Включать плановые префиксы</td>
      <td>
        <xsl:value-of select="@UsePlan"/>
      </td>
        </tr>
      <tr>
          <td>Включать фактические префиксы</td>
      <td>
        <xsl:value-of select="@UseFact"/>
      </td>
        </tr>
       <tr>
          <td>Исключенные предприятия</td>
      <td>
        <xsl:apply-templates  select="ExcludeObjects/item"/>
      </td>
        </tr>
       <tr>
          <td>Включенные предприятия</td>
      <td>
        <xsl:apply-templates select="IncludeObjects/item"/>
      </td>
        </tr>
      <tr>
          <td>Исключенные префиксы</td>
      <td>
        <xsl:apply-templates select="ExcludePrefixes/item"/>
      </td>
        </tr>
      <tr>
          <td>Включенные префиксы</td>
      <td>
        <xsl:apply-templates select="IncludePrefixes/item"/>
      </td>
        </tr>
      

     </table>
  </xsl:template>
  
  <xsl:template match="Parameters//item">
    <xsl:value-of select="."/>;&#160;
  
  </xsl:template>
    
  <xsl:template match="ProblemPrefixObjects/item">
    <tr>
      <td>
        <xsl:value-of select="position()"/>
      </td>
      <td>
        <xsl:value-of select="@ObjectId"/>
      </td>
      <td>
        <xsl:value-of select="@ObjectName"/>
      </td>
      <td class="problem">
        <table class="problem">
          <xsl:apply-templates select="./ProblemRecords/item"/>
        </table>
      </td>
    </tr>
  </xsl:template>

  <xsl:template match="ProblemRecords/item">
    <tr>
      <td class="prefix">
        <xsl:value-of select="@Prefix"/>
      </td>

      <td class="number">
        <xsl:value-of select="@Operators"/>
      </td>

      <td class="number">
        <xsl:value-of select="@Underwriters"/>
      </td>
       
    </tr>
  </xsl:template>
</xsl:stylesheet>

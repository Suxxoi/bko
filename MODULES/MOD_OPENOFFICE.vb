﻿Imports System
Imports System.IO

Module MOD_OPENOFFICE

    Sub insertIntoCell(ByVal strCellName, ByVal strText, ByVal objTable)
        Dim objCellText, objCellCursor As Object ' objects from OOo API 

        objCellText = objTable.getCellByName(strCellName)
        objCellCursor = objCellText.createTextCursor
        objCellCursor.setPropertyValue("CharColor", 0)
        objCellText.insertString(objCellCursor, strText, False)
    End Sub

    Public Sub OOO_SEND_PK(ByVal sID As String)
        On Error Resume Next
        Dim LNGIniFile As New IniFile(sLANGPATH)

        Dim rs As ADODB.Recordset
        Dim rs1 As ADODB.Recordset
        Dim sSQL, sSQL1, scN, cOTV As String

        Dim iA As String
        Dim iB As String
        Dim iC As String
        Dim iD As String
        Dim iE As String

        Call COUNT_FIELDS(sID)

        sSQL = "SELECT * FROM kompy WHERE TipTehn='PC' AND id=" & sID


        rs = New ADODB.Recordset
        rs.Open(sSQL, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

        Dim oSM As Object                 'Root object for accessing OpenOffice FROM VB
        Dim oDesk, oDoc As Object 'First objects FROM the API
        Dim arg(-1) As Object                 'Ignore it for the moment !
        Dim mmerge As Object
        Dim objCoreReflection As Object ' objects from OOo API 

        'Instanciate OOo : this line is mandatory with VB for OOo API
        oSM = CreateObject("com.sun.star.ServiceManager")
        'Create the first and most important service
        objCoreReflection = oSM.createInstance("com.sun.star.reflection.CoreReflection")

        oDesk = oSM.createInstance("com.sun.star.frame.Desktop")
        'Create a new doc

        oDoc = oDesk.loadComponentFROMURL("private:factory/swriter", "_blank", 0, arg)

        ' jon code
        Dim objText As Object, objCursor As Object

        Dim objTable As Object ' objects from OOo API 
        Dim objRows, objRow As Object

        objText = oDoc.GetText
        objCursor = objText.createTextCursor

        ' replace all
        Dim oSrch As Object

        Dim CONFIGURE As ADODB.Recordset

        Dim tiptehCP, uname, QWERT As String
        Dim GIST As Decimal = 0
        Dim intj As Decimal = 0

        With rs
            .MoveFirst()
            Do While Not .EOF
                tiptehCP = .Fields("TipTehn").Value
                GIST = 0

                objText.insertString(objCursor, " " & vbLf, False)
                objCursor.setPropertyValue("CharColor", 255)
                objCursor.setPropertyValue("CharShadowed", True)

                objText.insertString(objCursor, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG1", "") & ": " & sID & vbLf, False)
                objText.insertControlCharacter(objCursor, 0, False)

                objTable = oDoc.createInstance("com.sun.star.text.TextTable")
                objTable.Initialize(7, 2)

                'Insert the table
                objText.insertTextContent(objCursor, objTable, False)

                'Get first row
                objRows = objTable.GetRows
                objRow = objRows.getByIndex(0)

                'Set the table background color
                objTable.setPropertyValue("BackTransparent", False)
                objTable.setPropertyValue("BackColor", 16777215)

                'Set a different background color for the first row
                objRow.setPropertyValue("BackTransparent", False)
                objRow.setPropertyValue("BackColor", 16777215)

                'Fill the first table row

                insertIntoCell("A1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG2", ""), objTable)
                insertIntoCell("A2", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG3", ""), objTable)
                insertIntoCell("A3", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG4", ""), objTable)
                insertIntoCell("A4", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG5", ""), objTable)
                insertIntoCell("A5", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG6", ""), objTable)
                insertIntoCell("A6", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG7", ""), objTable)
                insertIntoCell("A7", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG8", ""), objTable)


                CONFIGURE = New ADODB.Recordset
                CONFIGURE.Open("SELECT * FROM CONFIGURE", DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly)
                With CONFIGURE
                    If Not IsDBNull(.Fields("ORG").Value) Then uname = .Fields("ORG").Value
                End With
                CONFIGURE.Close()
                CONFIGURE = Nothing

                cOTV = .Fields("OTvetstvennyj").Value

                insertIntoCell("B1", uname, objTable)
                insertIntoCell("B2", .Fields("filial").Value & "\" & .Fields("mesto").Value & "\" & .Fields("kabn").Value, objTable)
                insertIntoCell("B3", .Fields("TIP_COMPA").Value, objTable)
                insertIntoCell("B4", .Fields("OTvetstvennyj").Value, objTable)

                insertIntoCell("B5", .Fields("NET_NAME").Value, objTable)
                insertIntoCell("B6", .Fields("PSEVDONIM").Value, objTable)
                insertIntoCell("B7", .Fields("INV_NO_SYSTEM").Value, objTable)


                objText.insertControlCharacter(objCursor, 0, False)

                '33333333

                objTable = oDoc.createInstance("com.sun.star.text.TextTable")
                objTable.Initialize(iCOUNTFIELLDS + 1, 3)

                'Insert the table
                objText.insertControlCharacter(objCursor, 0, False)
                objText.insertTextContent(objCursor, objTable, False)

                'Get first row
                objRows = objTable.GetRows
                objRow = objRows.getByIndex(0)

                'Set the table background color
                objTable.setPropertyValue("BackTransparent", True)
                objTable.setPropertyValue("BackColor", 16777215)

                '333333


                'Формируем заголовок
                insertIntoCell("A1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG9", ""), objTable)
                insertIntoCell("B1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG10", ""), objTable)
                insertIntoCell("C1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG11", ""), objTable)
                'Mb
                insertIntoCell("A2", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG12", ""), objTable)
                insertIntoCell("B2", .Fields("Mb").Value & ", " & "SN: " & .Fields("Mb_Id").Value, objTable)
                insertIntoCell("C2", .Fields("Mb_Proizvod").Value, objTable)

                insertIntoCell("A3", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG13", ""), objTable)
                insertIntoCell("B3", .Fields("CPU1").Value & ", " & .Fields("CPUmhz1").Value, objTable)
                insertIntoCell("C3", .Fields("CPUProizv1").Value, objTable)

                intj = 4
                iA = "A" & intj
                iB = "B" & intj
                iC = "C" & intj

                If Len(.Fields("CPU2").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG13" & " 2", ""), objTable)
                    insertIntoCell(iB, .Fields("CPU2").Value & ", " & .Fields("CPUmhz2").Value, objTable)
                    insertIntoCell(iC, .Fields("CPUProizv2").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("CPU3").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG13" & " 3", ""), objTable)
                    insertIntoCell(iB, .Fields("CPU3").Value & ", " & .Fields("CPUmhz3").Value, objTable)
                    insertIntoCell(iC, .Fields("CPUProizv3").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("CPU4").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG13" & " 4", ""), objTable)
                    insertIntoCell(iB, .Fields("CPU4").Value & ", " & .Fields("CPUmhz4").Value, objTable)
                    insertIntoCell(iC, .Fields("CPUProizv4").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("RAM_1").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG14" & "", ""), objTable)
                    insertIntoCell(iB, .Fields("RAM_1").Value & ", " & .Fields("RAM_SN_1").Value, objTable)
                    insertIntoCell(iC, .Fields("RAM_PROIZV_1").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("RAM_2").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG14" & " 2", ""), objTable)
                    insertIntoCell(iB, .Fields("RAM_2").Value & ", " & "SN: " & .Fields("RAM_SN_2").Value, objTable)
                    insertIntoCell(iC, .Fields("RAM_PROIZV_2").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("RAM_3").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG14" & " 3", ""), objTable)
                    insertIntoCell(iB, .Fields("RAM_3").Value & ", " & "SN: " & .Fields("RAM_SN_3").Value, objTable)
                    insertIntoCell(iC, .Fields("RAM_PROIZV_3").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("RAM_4").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG14" & " 4", ""), objTable)
                    insertIntoCell(iB, .Fields("RAM_4").Value & ", " & "SN: " & .Fields("RAM_SN_4").Value, objTable)
                    insertIntoCell(iC, .Fields("RAM_PROIZV_4").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("HDD_Name_1").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG15", ""), objTable)
                    insertIntoCell(iB, .Fields("HDD_Name_1").Value & ", " & .Fields("HDD_OB_1").Value & ", SN: " & .Fields("HDD_SN_1").Value, objTable)
                    insertIntoCell(iC, .Fields("HDD_PROIZV_1").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("HDD_Name_2").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG15", ""), objTable)
                    insertIntoCell(iB, .Fields("HDD_Name_2").Value & ", " & .Fields("HDD_OB_2").Value & ", SN: " & .Fields("HDD_SN_2").Value, objTable)
                    insertIntoCell(iC, .Fields("HDD_PROIZV_2").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("HDD_Name_3").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG15", ""), objTable)
                    insertIntoCell(iB, .Fields("HDD_Name_3").Value & ", " & .Fields("HDD_OB_3").Value & ", SN: " & .Fields("HDD_SN_3").Value, objTable)
                    insertIntoCell(iC, .Fields("HDD_PROIZV_3").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("HDD_Name_4").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG15", ""), objTable)
                    insertIntoCell(iB, .Fields("HDD_Name_4").Value & ", " & .Fields("HDD_OB_4").Value & ", SN: " & .Fields("HDD_SN_4").Value, objTable)
                    insertIntoCell(iC, .Fields("HDD_PROIZV_4").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("FDD_NAME").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG16", ""), objTable)
                    insertIntoCell(iB, .Fields("FDD_NAME").Value & ", SN: " & .Fields("FDD_SN").Value, objTable)
                    insertIntoCell(iC, .Fields("FDD_PROIZV").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("SVGA_NAME").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG17", ""), objTable)
                    insertIntoCell(iB, .Fields("SVGA_NAME").Value & ", SN: " & .Fields("SVGA_SN").Value, objTable)
                    insertIntoCell(iC, .Fields("SVGA_PROIZV").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("SOUND_NAME").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG18", ""), objTable)
                    insertIntoCell(iB, .Fields("SOUND_NAME").Value & ", SN: " & .Fields("SOUND_SN").Value, objTable)
                    insertIntoCell(iC, .Fields("SOUND_PROIZV").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("CD_NAME").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG19", ""), objTable)
                    insertIntoCell(iB, .Fields("CD_NAME").Value & ", SN: " & .Fields("CD_SN").Value, objTable)
                    insertIntoCell(iC, .Fields("CD_PROIZV").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("CDRW_NAME").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG19", ""), objTable)
                    insertIntoCell(iB, .Fields("CDRW_NAME").Value & ", SN: " & .Fields("CDRW_SN").Value, objTable)
                    insertIntoCell(iC, .Fields("CDRW_PROIZV").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("DVD_NAME").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG19", ""), objTable)
                    insertIntoCell(iB, .Fields("DVD_NAME").Value & ", SN: " & .Fields("DVD_SN").Value, objTable)
                    insertIntoCell(iC, .Fields("DVD_PROIZV").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("NET_NAME_1").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG20", ""), objTable)
                    insertIntoCell(iB, .Fields("NET_NAME_1").Value & ", MAC: " & .Fields("NET_MAC_1").Value, objTable)
                    insertIntoCell(iC, .Fields("NET_PROIZV_1").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("NET_NAME_2").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG20", ""), objTable)
                    insertIntoCell(iB, .Fields("NET_NAME_2").Value & ", MAC: " & .Fields("NET_MAC_2").Value, objTable)
                    insertIntoCell(iC, .Fields("NET_PROIZV_2").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("BLOCK").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG21", ""), objTable)
                    insertIntoCell(iB, .Fields("BLOCK").Value & ", SN: " & .Fields("SN_BLOCK").Value, objTable)
                    insertIntoCell(iC, .Fields("PROIZV_BLOCK").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("KEYBOARD_NAME").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG22", ""), objTable)
                    insertIntoCell(iB, .Fields("KEYBOARD_NAME").Value & ", SN: " & .Fields("KEYBOARD_SN").Value, objTable)
                    insertIntoCell(iC, .Fields("KEYBOARD_PROIZV").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("MOUSE_NAME").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG23", ""), objTable)
                    insertIntoCell(iB, .Fields("MOUSE_NAME").Value & ", SN: " & .Fields("MOUSE_SN").Value, objTable)
                    insertIntoCell(iC, .Fields("MOUSE_PROIZV").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("MONITOR_NAME").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG24", ""), objTable)
                    insertIntoCell(iB, .Fields("MONITOR_NAME").Value & ", SN: " & .Fields("MONITOR_SN").Value, objTable)
                    insertIntoCell(iC, .Fields("MONITOR_PROIZV").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("MONITOR_NAME2").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG24", ""), objTable)
                    insertIntoCell(iB, .Fields("MONITOR_NAME2").Value & ", SN: " & .Fields("MONITOR_SN2").Value, objTable)
                    insertIntoCell(iC, .Fields("MONITOR_PROIZV2").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("AS_NAME").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG25", ""), objTable)
                    insertIntoCell(iB, .Fields("AS_NAME").Value & ", SN: " & .Fields("AS_SN").Value, objTable)
                    insertIntoCell(iC, .Fields("AS_PROIZV").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If


                If Len(.Fields("FILTR_NAME").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG26", ""), objTable)
                    insertIntoCell(iB, .Fields("FILTR_NAME").Value & ", SN: " & .Fields("FILTR_SN").Value, objTable)
                    insertIntoCell(iC, .Fields("FILTR_PROIZV").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If



                If Len(.Fields("PRINTER_NAME_1").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG27", ""), objTable)
                    insertIntoCell(iB, .Fields("PRINTER_NAME_1").Value & ", SN: " & .Fields("PRINTER_SN_1").Value, objTable)
                    insertIntoCell(iC, .Fields("PRINTER_PROIZV_1").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("PRINTER_NAME_2").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG27", ""), objTable)
                    insertIntoCell(iB, .Fields("PRINTER_NAME_2").Value & ", SN: " & .Fields("PRINTER_SN_2").Value, objTable)
                    insertIntoCell(iC, .Fields("PRINTER_PROIZV_2").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If

                If Len(.Fields("PRINTER_NAME_3").Value) = 0 Then
                Else
                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG27", ""), objTable)
                    insertIntoCell(iB, .Fields("PRINTER_NAME_3").Value & ", SN: " & .Fields("PRINTER_SN_3").Value, objTable)
                    insertIntoCell(iC, .Fields("PRINTER_PROIZV_3").Value, objTable)

                    intj = intj + 1
                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                End If






                objText.insertControlCharacter(objCursor, 0, False)
                objText.insertControlCharacter(objCursor, 0, False)


                .MoveNext()
            Loop
        End With

        rs.Close()
        rs = Nothing


        'В составе устройства

        sSQL1 = "SELECT count(*) as t_n FROM kompy WHERE PCL=" & sID

        rs1 = New ADODB.Recordset
        rs1.Open(sSQL1, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

        With rs1

            GIST = .Fields("t_n").Value

        End With

        rs1.Close()
        rs1 = Nothing

        If GIST = 0 Then

        Else

            objText.insertControlCharacter(objCursor, 0, False)

            objText.insertString(objCursor, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG28", "") & vbLf, False)

            objTable = oDoc.createInstance("com.sun.star.text.TextTable")
            objTable.Initialize(GIST + 1, 3)

            'Insert the table
            objText.insertControlCharacter(objCursor, 0, False)
            objText.insertTextContent(objCursor, objTable, False)

            'Get first row
            objRows = objTable.GetRows
            objRow = objRows.getByIndex(0)

            'Set the table background color
            objTable.setPropertyValue("BackTransparent", False)
            objTable.setPropertyValue("BackColor", 16777215)


            insertIntoCell("A1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG29", ""), objTable)
            insertIntoCell("B1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG10", ""), objTable)
            insertIntoCell("C1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG31", ""), objTable)


            sSQL1 = "SELECT * FROM kompy WHERE PCL=" & sID
            rs1 = New ADODB.Recordset
            rs1.Open(sSQL1, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

            intj = 2
            With rs1
                If .RecordCount <> 0 Then
                    .MoveFirst()
                    Do While Not .EOF

                        iA = "A" & intj
                        iB = "B" & intj
                        iC = "C" & intj


                        Select Case .Fields("tiptehn").Value

                            Case "Printer"

                                insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG27", ""), objTable)
                                insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("PRINTER_SN_1").Value, objTable)

                            Case "MFU"
                                insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG32", ""), objTable)
                                insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("PRINTER_SN_1").Value, objTable)

                            Case "MONITOR"

                                insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG24", ""), objTable)
                                insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("MONITOR_SN").Value, objTable)

                            Case "OT"

                                insertIntoCell(iA, .Fields("TIP_COMPA").Value, objTable)
                                insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("PRINTER_SN_1").Value, objTable)

                            Case "SCANER"

                                insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG34", ""), objTable)
                                insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("PRINTER_SN_1").Value, objTable)

                            Case "FS"

                                insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG26", ""), objTable)
                                insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("PRINTER_SN_1").Value, objTable)


                            Case "IBP"

                                insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG35", ""), objTable)
                                insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("PRINTER_SN_1").Value, objTable)

                            Case "SOUND"

                                insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG25", ""), objTable)
                                insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("PRINTER_SN_1").Value, objTable)

                            Case "USB"

                                insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG37", ""), objTable)
                                insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("PRINTER_SN_1").Value, objTable)


                            Case "MOUSE"

                                insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG23", ""), objTable)
                                insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("PRINTER_SN_1").Value, objTable)

                            Case "KEYB"

                                insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG22", ""), objTable)
                                insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("PRINTER_SN_1").Value, objTable)

                                'insertIntoCell(iD, .Fields("INV_NO_PRINTER").Value, objTable)

                        End Select



                        intj = intj + 1

                        .MoveNext()
                    Loop
                End If
            End With

            rs1.Close()
            rs1 = Nothing

            objText.insertString(objCursor, " " & vbLf, False)

        End If




        'Установленное ПО

        sSQL1 = "SELECT count(*) as t_n FROM SOFT_INSTALL WHERE id_comp=" & sID

        rs1 = New ADODB.Recordset
        rs1.Open(sSQL1, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

        With rs1

            GIST = .Fields("t_n").Value

        End With

        rs1.Close()
        rs1 = Nothing

        If GIST = 0 Then
        Else


            objTable = oDoc.createInstance("com.sun.star.text.TextTable")
            objTable.Initialize(GIST + 1, 1)
            objText.insertTextContent(objCursor, objTable, False)

            objRows = objTable.GetRows
            objRow = objRows.getByIndex(0)

            objTable.setPropertyValue("BackTransparent", True)
            objTable.setPropertyValue("BackColor", 16777215)


            intj = 1
            insertIntoCell("A1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG38", ""), objTable) 'lv_teh_fil_otd.SELECTedItem.Text, objTable

            sSQL1 = "SELECT * FROM SOFT_INSTALL WHERE id_comp=" & sID
            rs1 = New ADODB.Recordset
            rs1.Open(sSQL1, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
            With rs1
                If .RecordCount <> 0 Then
                    .MoveFirst()
                    Do While Not .EOF
                        iA = "A" & intj + 1

                        insertIntoCell(iA, .Fields("Soft"), objTable) 'lv_teh_fil_otd.SELECTedItem.Text, objTable
                        intj = intj + 1

                        .MoveNext()
                    Loop
                End If
            End With

            rs1.Close()
            rs1 = Nothing

            objText.insertString(objCursor, " " & vbLf, False)

        End If


        '#####################################################################################
        'ПЕРЕМЕЩЕНИЕ ТЕХНИКИ


        Dim rscount As ADODB.Recordset 'Объявляем рекордсет
        rscount = New ADODB.Recordset
        rscount.Open("SELECT COUNT(*) AS total_number FROM dvig WHERE id_comp=" & sID, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

        With rscount
            GIST = .Fields("total_number").Value
        End With

        rscount.Close()
        rscount = Nothing


        If GIST > 0 Then

            objText.insertControlCharacter(objCursor, 0, False)

            objText.insertString(objCursor, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG39", "") & vbLf, False)

            objTable = oDoc.createInstance("com.sun.star.text.TextTable")
            objTable.Initialize(GIST + 1, 4)

            'Insert the table
            objText.insertControlCharacter(objCursor, 0, False)
            objText.insertTextContent(objCursor, objTable, False)

            'Get first row
            objRows = objTable.GetRows
            objRow = objRows.getByIndex(0)

            'Set the table background color
            objTable.setPropertyValue("BackTransparent", False)
            objTable.setPropertyValue("BackColor", 16777215)

            'Формируем заголовок
            insertIntoCell("A1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG40", ""), objTable)
            insertIntoCell("B1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG41", ""), objTable)
            insertIntoCell("C1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG42", ""), objTable)
            insertIntoCell("D1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG43", ""), objTable)

            rscount = New ADODB.Recordset
            rscount.Open("SELECT * FROM dvig WHERE id_comp=" & sID & " ORDER by data", DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

            intj = 2
            With rscount
                .MoveFirst()
                Do While Not .EOF = True

                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                    iD = "D" & intj

                    insertIntoCell(iA, .Fields("oldMesto").Value, objTable)
                    insertIntoCell(iB, .Fields("NewMesto").Value, objTable)
                    insertIntoCell(iC, .Fields("Prich").Value, objTable)
                    insertIntoCell(iD, .Fields("data").Value, objTable)
                    intj = intj + 1


                    .MoveNext()
                    'DoEvents
                Loop
            End With
            rscount.Close()
            rscount = Nothing

        End If

        '#####################################################################################
        'РЕМОНТЫ

        rscount = New ADODB.Recordset
        rscount.Open("SELECT COUNT(*) AS total_number FROM Remont WHERE id_comp=" & sID, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

        With rscount
            GIST = .Fields("total_number").Value
        End With

        rscount.Close()
        rscount = Nothing


        If GIST > 0 Then


            objText.insertControlCharacter(objCursor, 0, False)

            objText.insertString(objCursor, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG44", "") & vbLf, False)

            objTable = oDoc.createInstance("com.sun.star.text.TextTable")
            objTable.Initialize(GIST + 1, 5)

            'Insert the table
            objText.insertControlCharacter(objCursor, 0, False)
            objText.insertTextContent(objCursor, objTable, False)

            'Get first row
            objRows = objTable.GetRows
            objRow = objRows.getByIndex(0)

            'Set the table background color
            objTable.setPropertyValue("BackTransparent", False)
            objTable.setPropertyValue("BackColor", 16777215)


            insertIntoCell("A1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG43", ""), objTable)
            insertIntoCell("B1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG46", ""), objTable)
            insertIntoCell("C1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG47", ""), objTable)
            insertIntoCell("D1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG48", ""), objTable)
            insertIntoCell("E1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG49", ""), objTable)

            rscount = New ADODB.Recordset
            rscount.Open("SELECT * FROM Remont WHERE id_comp=" & sID & " ORDER by Date", DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

            'Dim A As String

            intj = 2
            With rscount
                .MoveFirst()
                Do While Not .EOF = True

                    iA = "A" & intj
                    iB = "B" & intj
                    iC = "C" & intj
                    iD = "D" & intj
                    iE = "E" & intj
                    insertIntoCell(iA, .Fields("Date").Value, objTable)
                    insertIntoCell(iB, .Fields("Remont").Value, objTable)
                    insertIntoCell(iC, .Fields("Master").Value, objTable)
                    insertIntoCell(iD, .Fields("Uroven").Value, objTable)
                    insertIntoCell(iE, .Fields("vip").Value, objTable)
                    intj = intj + 1


                    .MoveNext()
                    'DoEvents
                Loop
            End With
            rscount.Close()
            rscount = Nothing

        End If



        objText.insertString(objCursor, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG50", "") & vbLf, False)


        objTable = oDoc.createInstance("com.sun.star.text.TextTable")
        objTable.Initialize(2, 2)

        'Insert the table
        objText.insertTextContent(objCursor, objTable, False)

        'Get first row
        objRows = objTable.GetRows
        objRow = objRows.getByIndex(0)

        'Set the table background color
        objTable.setPropertyValue("BackTransparent", False)
        objTable.setPropertyValue("BackColor", 16777215)

        'Set a different background color for the first row
        'objRow.setPropertyValue "BackTransparent", False
        'objRow.setPropertyValue "BackColor", vbWhite


        CONFIGURE = New ADODB.Recordset
        CONFIGURE.Open("SELECT * FROM CONFIGURE", DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
        With CONFIGURE
            If .RecordCount <> 0 Then
                If Not IsDBNull(.Fields("SISADM")) Then uname = .Fields("SISADM").Value
            Else
                uname = "Nothing"
            End If
        End With
        CONFIGURE.Close()
        CONFIGURE = Nothing



        'Формируем заголовок
        insertIntoCell("A1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG51", ""), objTable)
        insertIntoCell("B1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG52", ""), objTable)

        insertIntoCell("A2", uname, objTable)
        insertIntoCell("B2", cOTV, objTable)






    End Sub

    Public Sub blanks_my_o(ByVal tipot As String)
        'Dim tipot As String
        Dim sSQL, sSQL1, SISADM, Organ As String
        Dim LNGIniFile As New IniFile(sLANGPATH)
        'Указать файл необхадимо
        Dim uname As String

        'tipot = frmMain.lbl_path 'App.Path & "\blanks\blanks_my.dot"



        If Len(tipot) = 0 Then Exit Sub
        On Error Resume Next

        Dim rs1 As ADODB.Recordset
        sSQL1 = "SELECT * FROM CONFIGURE"
        rs1 = New ADODB.Recordset
        rs1.Open(sSQL1, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

        With rs1
            Organ = .Fields("org").Value
            SISADM$ = .Fields("SISADM").Value
        End With
        rs1.Close()
        rs1 = Nothing


        Dim rs As ADODB.Recordset
        sSQL = "SELECT * FROM kompy WHERE id = " & frmComputers.sCOUNT
        rs = New ADODB.Recordset
        rs.Open(sSQL, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

        Dim intj As String
        Dim A, B, C, D, E As String


        Dim oSM As Object            'Root object for accessing OpenOffice FROM VB
        Dim oDesk, oDoc As Object 'First objects FROM the API
        Dim arg(-1) As Object             'Ignore it for the moment !
        Dim mmerge As Object
        'Instanciate OOo : this line is mandatory with VB for OOo API

        Dim objCoreReflection As Object ' objects from OOo API 
        'Dim objText As Object, objCursor As Object

        Dim objTable As Object ' objects from OOo API 
        Dim objRows, objRow As Object

        oSM = CreateObject("com.sun.star.ServiceManager")


        'Create the first and most important service
        oDesk = oSM.createInstance("com.sun.star.frame.Desktop")
        'Create a new doc
        oDoc = oDesk.loadComponentFROMURL("file:///" & tipot, "_blank", 0, arg)
        ' jon code
        Dim objText As Object, objCursor As Object
        objText = oDoc.GetText
        objCursor = objText.createTextCursor

        ' replace all
        Dim oSrch As Object
        With rs
            .MoveFirst()
            Do While Not .EOF = True
                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("Организация")
                oSrch.setReplaceString(Organ)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("Сисадмин")
                oSrch.setReplaceString(SISADM$)
                Debug.Print(oDoc.replaceAll(oSrch))

                'Ser_N_SIS
                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("Ser_N_SIS")
                oSrch.setReplaceString(.Fields("Ser_N_SIS"))
                Debug.Print(oDoc.replaceAll(oSrch))


                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("ID")
                oSrch.setReplaceString(.Fields("ID"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("nomerPC")
                oSrch.setReplaceString(.Fields("nomerPC"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CPU1")
                oSrch.setReplaceString(.Fields("CPU1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CPUmhz1")
                oSrch.setReplaceString(.Fields("CPUmhz1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CPUSocket1")
                oSrch.setReplaceString(.Fields("CPUSocket1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CPUProizv1")
                oSrch.setReplaceString(.Fields("CPUProizv1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CPU2")
                oSrch.setReplaceString(.Fields("CPU2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CPUmhz2")
                oSrch.setReplaceString(.Fields("CPUmhz2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CPUSocket2")
                oSrch.setReplaceString(.Fields("CPUSocket2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CPUProizv2")
                oSrch.setReplaceString(.Fields("CPUProizv2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CPU3")
                oSrch.setReplaceString(.Fields("CPU3"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CPUmhz3")
                oSrch.setReplaceString(.Fields("CPUmhz3"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CPUSocket3")
                oSrch.setReplaceString(.Fields("CPUSocket3"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CPUProizv3")
                oSrch.setReplaceString(.Fields("CPUProizv3"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CPU4")
                oSrch.setReplaceString(.Fields("CPU4"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CPUmhz4")
                oSrch.setReplaceString(.Fields("CPUmhz4"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CPUSocket4")
                oSrch.setReplaceString(.Fields("CPUSocket4"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CPUProizv4")
                oSrch.setReplaceString(.Fields("CPUProizv4"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("Mb1")
                oSrch.setReplaceString(.Fields("Mb"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("Mb_Chip")
                oSrch.setReplaceString(.Fields("Mb_Chip"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("Mb_Iz")
                oSrch.setReplaceString(.Fields("Mb_Id"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("Mb_Proizvod")
                oSrch.setReplaceString(.Fields("Mb_Proizvod"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("RAM_1")
                oSrch.setReplaceString(.Fields("RAM_1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("RAM_SN_1")
                oSrch.setReplaceString(.Fields("RAM_SN_1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("RAM_speed_1")
                oSrch.setReplaceString(.Fields("RAM_speed_1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("RAM_PROIZV_1")
                oSrch.setReplaceString(.Fields("RAM_PROIZV_1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("RAM_2")
                oSrch.setReplaceString(.Fields("RAM_2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("RAM_SN_2")
                oSrch.setReplaceString(.Fields("RAM_SN_2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("RAM_speed_2")
                oSrch.setReplaceString(.Fields("RAM_speed_2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("RAM_PROIZV_2")
                oSrch.setReplaceString(.Fields("RAM_PROIZV_2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("RAM_3")
                oSrch.setReplaceString(.Fields("RAM_3"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("RAM_SN_3")
                oSrch.setReplaceString(.Fields("RAM_SN_3"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("RAM_speed_3")
                oSrch.setReplaceString(.Fields("RAM_speed_3"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("RAM_PROIZV_3")
                oSrch.setReplaceString(.Fields("RAM_PROIZV_3"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("RAM_4")
                oSrch.setReplaceString(.Fields("RAM_4"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("RAM_SN_4")
                oSrch.setReplaceString(.Fields("RAM_SN_4"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("RAM_speed_4")
                oSrch.setReplaceString(.Fields("RAM_speed_4"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("RAM_PROIZV_4")
                oSrch.setReplaceString(.Fields("RAM_PROIZV_4"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("HDD_Name_1")
                oSrch.setReplaceString(.Fields("HDD_Name_1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("HDD_OB_1")
                oSrch.setReplaceString(.Fields("HDD_OB_1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("HDD_SN_1")
                oSrch.setReplaceString(.Fields("HDD_SN_1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("HDD_PROIZV_1")
                oSrch.setReplaceString(.Fields("HDD_PROIZV_1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("HDD_Name_2")
                oSrch.setReplaceString(.Fields("HDD_Name_2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("HDD_OB_2")
                oSrch.setReplaceString(.Fields("HDD_OB_2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("HDD_SN_2")
                oSrch.setReplaceString(.Fields("HDD_SN_2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("HDD_PROIZV_2")
                oSrch.setReplaceString(.Fields("HDD_PROIZV_2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("HDD_Name_3")
                oSrch.setReplaceString(.Fields("HDD_Name_3"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("HDD_OB_3")
                oSrch.setReplaceString(.Fields("HDD_OB_3"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("HDD_SN_3")
                oSrch.setReplaceString(.Fields("HDD_SN_3"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("HDD_PROIZV_3")
                oSrch.setReplaceString(.Fields("HDD_PROIZV_3"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("HDD_Name_4")
                oSrch.setReplaceString(.Fields("HDD_Name_4"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("HDD_OB_4")
                oSrch.setReplaceString(.Fields("HDD_OB_4"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("HDD_SN_4")
                oSrch.setReplaceString(.Fields("HDD_SN_4"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("HDD_PROIZV_4")
                oSrch.setReplaceString(.Fields("HDD_PROIZV_4"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("SVGA_NAME")
                oSrch.setReplaceString(.Fields("SVGA_NAME"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("SVGA_OB_RAM")
                oSrch.setReplaceString(.Fields("SVGA_OB_RAM"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("SVGA_SN")
                oSrch.setReplaceString(.Fields("SVGA_SN"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("SVGA_PROIZV")
                oSrch.setReplaceString(.Fields("SVGA_PROIZV"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("SOUND_NAME")
                oSrch.setReplaceString(.Fields("SOUND_NAME"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("SOUND_SN")
                oSrch.setReplaceString(.Fields("SOUND_SN"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("SOUND_PROIZV")
                oSrch.setReplaceString(.Fields("SOUND_PROIZV"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CD_NAME")
                oSrch.setReplaceString(.Fields("CD_NAME"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CD_SPEED")
                oSrch.setReplaceString(.Fields("CD_SPEED"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CD_SN")
                oSrch.setReplaceString(.Fields("CD_SN"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CD_PROIZV")
                oSrch.setReplaceString(.Fields("CD_PROIZV"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CDRW_NAME")
                oSrch.setReplaceString(.Fields("CDRW_NAME"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CDRW_SPEED")
                oSrch.setReplaceString(.Fields("CDRW_SPEED"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CDRW_SN")
                oSrch.setReplaceString(.Fields("CDRW_SN"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("CDRW_PROIZV")
                oSrch.setReplaceString(.Fields("CDRW_PROIZV"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("DVD_NAME")
                oSrch.setReplaceString(.Fields("DVD_NAME"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("DVD_SPEED")
                oSrch.setReplaceString(.Fields("DVD_SPEED"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("DVD_SN")
                oSrch.setReplaceString(.Fields("DVD_SN"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("DVD_PROIZV")
                oSrch.setReplaceString(.Fields("DVD_PROIZV"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("NET_NAME_1")
                oSrch.setReplaceString(.Fields("NET_NAME_1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("NET_IP_1")
                oSrch.setReplaceString(.Fields("NET_IP_1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("NET_MAC_1")
                oSrch.setReplaceString(.Fields("NET_MAC_1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("NET_PROIZV_1")
                oSrch.setReplaceString(.Fields("NET_PROIZV_1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("NET_NAME_2")
                oSrch.setReplaceString(.Fields("NET_NAME_2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("NET_IP_2")
                oSrch.setReplaceString(.Fields("NET_IP_2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("NET_MAC_2")
                oSrch.setReplaceString(.Fields("NET_MAC_2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("NET_PROIZV_2")
                oSrch.setReplaceString(.Fields("NET_PROIZV_2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("FDD_NAME")
                oSrch.setReplaceString(.Fields("FDD_NAME"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("FDD_SN")
                oSrch.setReplaceString(.Fields("FDD_SN"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("FDD_PROIZV")
                oSrch.setReplaceString(.Fields("FDD_PROIZV"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("MODEM_NAME")
                oSrch.setReplaceString(.Fields("MODEM_NAME"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("MODEM_SN")
                oSrch.setReplaceString(.Fields("MODEM_SN"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("MODEM_PROIZV")
                oSrch.setReplaceString(.Fields("MODEM_PROIZV"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("KEYBOARD_NAME")
                oSrch.setReplaceString(.Fields("KEYBOARD_NAME"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("KEYBOARD_SN")
                oSrch.setReplaceString(.Fields("KEYBOARD_SN"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("KEYBOARD_PROIZV")
                oSrch.setReplaceString(.Fields("KEYBOARD_PROIZV"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("MOUSE_NAME")
                oSrch.setReplaceString(.Fields("MOUSE_NAME"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("MOUSE_SN")
                oSrch.setReplaceString(.Fields("MOUSE_SN"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("MOUSE_PROIZV")
                oSrch.setReplaceString(.Fields("MOUSE_PROIZV"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("USB_NAME")
                oSrch.setReplaceString(.Fields("USB_NAME"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("USB_SN")
                oSrch.setReplaceString(.Fields("USB_SN"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("USB_PROIZV")
                oSrch.setReplaceString(.Fields("USB_PROIZV"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("PCI_NAME")
                oSrch.setReplaceString(.Fields("PCI_NAME"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("PCI_SN")
                oSrch.setReplaceString(.Fields("PCI_SN"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("PCI_SN")
                oSrch.setReplaceString(.Fields("PCI_SN"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("PCI_PROIZV")
                oSrch.setReplaceString(.Fields("PCI_PROIZV"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("MONITOR_NAME")
                oSrch.setReplaceString(.Fields("MONITOR_NAME"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("MONITOR_DUM")
                oSrch.setReplaceString(.Fields("MONITOR_DUM"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("MONITOR_SN")
                oSrch.setReplaceString(.Fields("MONITOR_SN"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("MONITOR_PROIZV")
                oSrch.setReplaceString(.Fields("MONITOR_PROIZV"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("MONITOR_NAME2")
                oSrch.setReplaceString(.Fields("MONITOR_NAME2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("MONITOR_DUM2")
                oSrch.setReplaceString(.Fields("MONITOR_DUM2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("MONITOR_SN2")
                oSrch.setReplaceString(.Fields("MONITOR_SN2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("MONITOR_PROIZV2")
                oSrch.setReplaceString(.Fields("MONITOR_PROIZV2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("AS_NAME")
                oSrch.setReplaceString(.Fields("AS_NAME"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("AS_SN")
                oSrch.setReplaceString(.Fields("AS_SN"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("AS_PROIZV")
                oSrch.setReplaceString(.Fields("AS_PROIZV"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("IBP_NAME")
                oSrch.setReplaceString(.Fields("IBP_NAME"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("IBP_SN")
                oSrch.setReplaceString(.Fields("IBP_SN"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("IBP_PROIZV")
                oSrch.setReplaceString(.Fields("IBP_PROIZV"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("FILTR_NAME")
                oSrch.setReplaceString(.Fields("FILTR_NAME"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("FILTR_SN")
                oSrch.setReplaceString(.Fields("FILTR_SN"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("FILTR_PROIZV")
                oSrch.setReplaceString(.Fields("FILTR_PROIZV"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("PRINTER_NAME_1")
                oSrch.setReplaceString(.Fields("PRINTER_NAME_1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("PRINTER_SN_1")
                oSrch.setReplaceString(.Fields("PRINTER_SN_1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("PORT_1")
                oSrch.setReplaceString(.Fields("PORT_1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("PRINTER_PROIZV_1")
                oSrch.setReplaceString(.Fields("PRINTER_PROIZV_1"))
                Debug.Print(oDoc.replaceAll(oSrch))

                '#########

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("PRINTER_NAME_2")
                oSrch.setReplaceString(.Fields("PRINTER_NAME_2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("PRINTER_SN_2")
                oSrch.setReplaceString(.Fields("PRINTER_SN_2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("PORT_2")
                oSrch.setReplaceString(.Fields("PORT_2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("PRINTER_PROIZV_2")
                oSrch.setReplaceString(.Fields("PRINTER_PROIZV_2"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("PRINTER_NAME_3")
                oSrch.setReplaceString(.Fields("PRINTER_NAME_3"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("PRINTER_SN_3")
                oSrch.setReplaceString(.Fields("PRINTER_SN_3"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("PORT_3")
                oSrch.setReplaceString(.Fields("PORT_3"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("PRINTER_PROIZV_3")
                oSrch.setReplaceString(.Fields("PRINTER_PROIZV_3"))
                Debug.Print(oDoc.replaceAll(oSrch))

                '#########
                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("SCANER_NAME")
                oSrch.setReplaceString(.Fields("SCANER_NAME"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("SCANER_SN")
                oSrch.setReplaceString(.Fields("SCANER_SN"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("SCANER_PROIZV")
                oSrch.setReplaceString(.Fields("SCANER_PROIZV"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("NET_NAME")
                oSrch.setReplaceString(.Fields("NET_NAME"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("PSEVDONIM")
                oSrch.setReplaceString(.Fields("PSEVDONIM"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("Department")
                oSrch.setReplaceString(.Fields("MESTO"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("Office")
                oSrch.setReplaceString(.Fields("kabn"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("TIP_COMPA")
                oSrch.setReplaceString(.Fields("TIP_COMPA"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("Branch")
                oSrch.setReplaceString(.Fields("FILIAL"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("TELEPHONE")
                oSrch.setReplaceString(.Fields("TELEPHONE"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("INV_NO_SYSTEM")
                oSrch.setReplaceString(.Fields("INV_NO_SYSTEM"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("INV_NO_PRINTER")
                oSrch.setReplaceString(.Fields("INV_NO_PRINTER"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("INV_NO_MODEM")
                oSrch.setReplaceString(.Fields("INV_NO_MODEM"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("INV_NO_SCANER")
                oSrch.setReplaceString(.Fields("INV_NO_SCANER"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("INV_NO_MONITOR")
                oSrch.setReplaceString(.Fields("INV_NO_MONITOR"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("INV_NO_IBP")
                oSrch.setReplaceString(.Fields("INV_NO_IBP"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString(LNGIniFile.GetString("MOD_OPENOFFICE", "MSG5", ""))
                oSrch.setReplaceString(.Fields("OTvetstvennyj"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("BLOCK")
                oSrch.setReplaceString(.Fields("BLOCK"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("SN_BLOCK")
                oSrch.setReplaceString(.Fields("SN_BLOCK"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("PROIZV_BLOCK")
                oSrch.setReplaceString(.Fields("PROIZV_BLOCK"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("SYS_PR")
                oSrch.setReplaceString(.Fields("SYS_PR"))
                Debug.Print(oDoc.replaceAll(oSrch))


                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("StoimRub")
                oSrch.setReplaceString(.Fields("StoimRub"))
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("DataVVoda")
                oSrch.setReplaceString(.Fields("DataVVoda"))
                Debug.Print(oDoc.replaceAll(oSrch))


                Dim rs2 As ADODB.Recordset
                rs2 = New ADODB.Recordset
                rs2.Open("Select Postav FROM Garantia_sis WHERE Id_Comp=" & .Fields("id").Value, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)


                With rs2
                    uname = .Fields("Postav").Value
                End With
                rs2.Close()
                rs2 = Nothing

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("Postav")
                oSrch.setReplaceString(uname)
                Debug.Print(oDoc.replaceAll(oSrch))


                objText.insertControlCharacter(objCursor, 0, False)


                .MoveNext()
                'DoEvents
            Loop
        End With
        rs.Close()
        rs = Nothing



        Dim objIniFile As New IniFile(PrPath & "base.ini")

        uname = objIniFile.GetString("MYBLANK", "VSU", "0")

        If uname = 1 Then
            'В составе устройства
            '#####################################################################################

            sSQL1 = "SELECT count(*) as t_n FROM kompy WHERE PCL=" & frmComputers.sCOUNT
            Dim GIST As String

            rs1 = New ADODB.Recordset
            rs1.Open(sSQL1, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

            With rs1

                GIST = .Fields("t_n").Value

            End With

            rs1.Close()
            rs1 = Nothing

            If GIST = 0 Then

            Else
                Dim iA, iB, iC, iD As String

                objText.insertControlCharacter(objCursor, 0, False)
                objText.insertString(objCursor, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG28", "") & vbLf, False)


                objTable = oDoc.createInstance("com.sun.star.text.TextTable")
                objTable.Initialize(GIST + 1, 4)
                objText.insertTextContent(objCursor, objTable, False)

                objRows = objTable.GetRows
                objRow = objRows.getByIndex(0)

                objTable.setPropertyValue("BackTransparent", True)
                objTable.setPropertyValue("BackColor", 16777215)


                intj = 1


                insertIntoCell("A1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG28", ""), objTable) 'lv_teh_fil_otd.SELECTedItem.Text, objTable
                insertIntoCell("B1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG10", ""), objTable) 'lv_teh_fil_otd.SELECTedItem.Text, objTable
                insertIntoCell("C1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG31", ""), objTable) 'lv_teh_fil_otd.SELECTedItem.Text, objTable
                insertIntoCell("D1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG8", ""), objTable) 'lv_teh_fil_otd.SELECTedItem.Text, objTable




                sSQL1 = "SELECT * FROM kompy WHERE PCL=" & frmComputers.sCOUNT
                rs1 = New ADODB.Recordset
                rs1.Open(sSQL1, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
                With rs1
                    If .RecordCount <> 0 Then
                        .MoveFirst()
                        Do While Not .EOF
                            iA = "A" & intj + 1
                            iB = "B" & intj + 1
                            iC = "C" & intj + 1
                            iD = "C" & intj + 1

                            Select Case .Fields("tiptehn").Value

                                Case "Printer"

                                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG27", ""), objTable)
                                    insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                    insertIntoCell(iC, .Fields("PRINTER_SN_1").Value, objTable)
                                    insertIntoCell(iD, .Fields("INV_NO_PRINTER").Value, objTable)


                                Case "MFU"
                                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG32", ""), objTable)
                                    insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                    insertIntoCell(iC, .Fields("PRINTER_SN_1").Value, objTable)
                                    insertIntoCell(iD, .Fields("INV_NO_PRINTER").Value, objTable)

                                Case "MONITOR"

                                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG24", ""), objTable)
                                    insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                    insertIntoCell(iC, .Fields("MONITOR_SN").Value, objTable)
                                    insertIntoCell(iD, .Fields("INV_NO_MONITOR").Value, objTable)

                                Case "OT"

                                    insertIntoCell(iA, .Fields("TIP_COMPA").Value, objTable)
                                    insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                    insertIntoCell(iC, .Fields("PRINTER_SN_1").Value, objTable)
                                    insertIntoCell(iD, .Fields("INV_NO_PRINTER").Value, objTable)

                                Case "SCANER"

                                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG34", ""), objTable)
                                    insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                    insertIntoCell(iC, .Fields("PRINTER_SN_1").Value, objTable)
                                    insertIntoCell(iD, .Fields("INV_NO_PRINTER").Value, objTable)

                                Case "FS"

                                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG26", ""), objTable)
                                    insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                    insertIntoCell(iC, .Fields("PRINTER_SN_1").Value, objTable)
                                    insertIntoCell(iD, .Fields("INV_NO_PRINTER").Value, objTable)


                                Case "IBP"

                                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG35", ""), objTable)
                                    insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                    insertIntoCell(iC, .Fields("PRINTER_SN_1").Value, objTable)
                                    insertIntoCell(iD, .Fields("INV_NO_PRINTER").Value, objTable)

                                Case "SOUND"

                                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG25", ""), objTable)
                                    insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                    insertIntoCell(iC, .Fields("PRINTER_SN_1").Value, objTable)
                                    insertIntoCell(iD, .Fields("INV_NO_PRINTER").Value, objTable)

                                Case "USB"

                                    insertIntoCell(iA, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG37", ""), objTable)
                                    insertIntoCell(iB, .Fields("NET_NAME").Value, objTable)
                                    insertIntoCell(iC, .Fields("PRINTER_SN_1").Value, objTable)
                                    insertIntoCell(iD, .Fields("INV_NO_PRINTER").Value, objTable)

                            End Select



                            intj = intj + 1




                            .MoveNext()
                        Loop
                    End If
                End With

                rs1.Close()
                rs1 = Nothing

                objText.insertString(objCursor, " " & vbLf, False)

            End If
        End If

        Dim rscount As ADODB.Recordset 'Объявляем рекордсет
        Dim Coun1 As Long


        uname = objIniFile.GetString("MYBLANK", "POu", "0")

        If uname = 1 Then

            '#####################################################################################
            'Установленное ПО

            rscount = New ADODB.Recordset
            rscount.Open("SELECT COUNT(*) AS total_number FROM SOFT_INSTALL WHERE id_comp=" & frmComputers.sCOUNT, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

            With rscount
                Coun1 = .Fields("total_number").Value
            End With

            rscount.Close()
            rscount = Nothing


            If Coun1 > 0 Then

                objText.insertControlCharacter(objCursor, 0, False)
                objText.insertString(objCursor, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG38", "") & vbLf, False)


                objTable = oDoc.createInstance("com.sun.star.text.TextTable")
                objTable.Initialize(Coun1 + 1, 1)

                'Insert the table
                objText.insertControlCharacter(objCursor, 0, False)
                objText.insertTextContent(objCursor, objTable, False)

                'Get first row
                objRows = objTable.GetRows
                objRow = objRows.getByIndex(0)

                'Set the table background color
                objTable.setPropertyValue("BackTransparent", False)
                objTable.setPropertyValue("BackColor", Color.White)

                'Set a different background color for the first row
                'objRow.setPropertyValue "BackTransparent", False
                'objRow.setPropertyValue "BackColor", vbWhite

                'frmContacts.SoftClick()
                'Формируем заголовок
                insertIntoCell("A1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG38", ""), objTable)

                rscount = New ADODB.Recordset
                rscount.Open("SELECT * FROM SOFT_INSTALL WHERE id_comp=" & frmComputers.sCOUNT, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

                'Dim A As String

                intj = 2
                With rscount
                    .MoveFirst()
                    Do While Not .EOF = True

                        A = "A" & intj
                        insertIntoCell(A, .Fields("Soft"), objTable)
                        intj = intj + 1


                        .MoveNext()
                        'DoEvents
                    Loop
                End With
                rscount.Close()
                rscount = Nothing


                objText.insertControlCharacter(objCursor, 0, False)

            End If

        End If


        '#####################################################################################
        'ПЕРЕМЕЩЕНИЕ ТЕХНИКИ


        uname = objIniFile.GetString("MYBLANK", "DVIG", "0")

        If uname = 1 Then


            'Dim rscount As ADODB.Recordset 'Объявляем рекордсет
            rscount = New ADODB.Recordset
            rscount.Open("SELECT COUNT(*) AS total_number FROM dvig WHERE id_comp=" & frmComputers.sCOUNT, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

            With rscount
                Coun1 = .Fields("total_number").Value
            End With

            rscount.Close()
            rscount = Nothing


            If Coun1 > 0 Then


                objText.insertControlCharacter(objCursor, 0, False)

                objText.insertString(objCursor, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG39", "") & vbLf, False)

                objTable = oDoc.createInstance("com.sun.star.text.TextTable")
                objTable.Initialize(Coun1 + 1, 4)

                'Insert the table
                objText.insertControlCharacter(objCursor, 0, False)
                objText.insertTextContent(objCursor, objTable, False)

                'Get first row
                objRows = objTable.GetRows
                objRow = objRows.getByIndex(0)

                'Set the table background color
                objTable.setPropertyValue("BackTransparent", False)
                objTable.setPropertyValue("BackColor", Color.White)

                'Set a different background color for the first row
                'objRow.setPropertyValue "BackTransparent", False
                'objRow.setPropertyValue "BackColor", vbWhite

                'Формируем заголовок
                insertIntoCell("A1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG40", ""), objTable)
                insertIntoCell("B1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG41", ""), objTable)
                insertIntoCell("C1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG42", ""), objTable)
                insertIntoCell("D1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG43", ""), objTable)

                rscount = New ADODB.Recordset
                rscount.Open("SELECT * FROM dvig WHERE id_comp=" & frmComputers.sCOUNT, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)


                intj = 2
                With rscount
                    .MoveFirst()
                    Do While Not .EOF = True

                        A = "A" & intj
                        B = "B" & intj
                        C = "C" & intj
                        D = "D" & intj

                        insertIntoCell(A, .Fields("oldMesto"), objTable)
                        insertIntoCell(B, .Fields("NewMesto"), objTable)
                        insertIntoCell(C, .Fields("Prich"), objTable)
                        insertIntoCell(D, .Fields("data"), objTable)
                        intj = intj + 1


                        .MoveNext()
                        'DoEvents
                    Loop
                End With
                rscount.Close()
                rscount = Nothing

            End If
        End If


        '#####################################################################################
        'РЕМОНТЫ


        uname = objIniFile.GetString("MYBLANK", "REMONT", "0")

        If uname = 1 Then

            rscount = New ADODB.Recordset
            rscount.Open("SELECT COUNT(*) AS total_number FROM Remont WHERE id_comp=" & frmComputers.sCOUNT, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

            With rscount
                Coun1 = .Fields("total_number").Value
            End With

            rscount.Close()
            rscount = Nothing


            If Coun1 > 0 Then


                objText.insertControlCharacter(objCursor, 0, False)

                objText.insertString(objCursor, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG44", "") & vbLf, False)

                objTable = oDoc.createInstance("com.sun.star.text.TextTable")
                objTable.Initialize(Coun1 + 1, 5)

                'Insert the table
                objText.insertControlCharacter(objCursor, 0, False)
                objText.insertTextContent(objCursor, objTable, False)

                'Get first row
                objRows = objTable.GetRows
                objRow = objRows.getByIndex(0)

                'Set the table background color
                objTable.setPropertyValue("BackTransparent", False)
                objTable.setPropertyValue("BackColor", Color.White)


                insertIntoCell("A1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG43", ""), objTable)
                insertIntoCell("B1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG46", ""), objTable)
                insertIntoCell("C1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG47", ""), objTable)
                insertIntoCell("D1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG48", ""), objTable)
                insertIntoCell("E1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG49", ""), objTable)

                rscount = New ADODB.Recordset
                rscount.Open("SELECT * FROM Remont WHERE id_comp=" & frmComputers.sCOUNT, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

                intj = 2
                With rscount
                    .MoveFirst()
                    Do While Not .EOF = True

                        A = "A" & intj
                        B = "B" & intj
                        C = "C" & intj
                        D = "D" & intj
                        E = "E" & intj
                        insertIntoCell(A, .Fields("Date"), objTable)
                        insertIntoCell(B, .Fields("Remont"), objTable)
                        insertIntoCell(C, .Fields("Name"), objTable)
                        insertIntoCell(D, .Fields("Uroven"), objTable)
                        insertIntoCell(E, .Fields("vip"), objTable)
                        intj = intj + 1


                        .MoveNext()
                        'DoEvents
                    Loop
                End With
                rscount.Close()
                rscount = Nothing

            End If
        End If


    End Sub

    Public Sub PASSWORD_MFU(ByVal sSID As String, ByVal sTEH As String)

        Dim tipot, SIS, sSQL As String

        Dim CONFIGURE As ADODB.Recordset


        Select Case sTEH

            Case "Printer"

                tipot = Directory.GetParent(Application.ExecutablePath).ToString & "\blanks\pswgl4.dot"

            Case "MFU"

                tipot = Directory.GetParent(Application.ExecutablePath).ToString & "\blanks\pswgl5.dot"

            Case "KOpir"
                tipot = Directory.GetParent(Application.ExecutablePath).ToString & "\blanks\pswgl3.dot"

        End Select




        CONFIGURE = New ADODB.Recordset
        CONFIGURE.Open("SELECT * FROM CONFIGURE", DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
        With CONFIGURE
            SIS = .Fields("sisadm").Value
        End With
        CONFIGURE.Close()
        CONFIGURE = Nothing


        Select Case sOfficePACK

            Case "OpenOffice.org"
                Dim oSM As Object            'Root object for accessing OpenOffice FROM VB
                Dim oDesk, oDoc As Object 'First objects FROM the API
                Dim arg(-1) As Object             'Ignore it for the moment !

                oSM = CreateObject("com.sun.star.ServiceManager")


                'Create the first and most important service
                oDesk = oSM.createInstance("com.sun.star.frame.Desktop")
                'Create a new doc
                oDoc = oDesk.loadComponentFROMURL("file:///" & tipot, "_blank", 0, arg)
                ' jon code
                Dim objText As Object, objCursor As Object
                objText = oDoc.GetText
                objCursor = objText.createTextCursor

                ' replace all
                Dim oSrch As Object

                Dim rs As ADODB.Recordset
                sSQL = "SELECT * FROM kompy WHERE id = " & sSID
                rs = New ADODB.Recordset
                rs.Open(sSQL, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)


                With rs
                    'Set oSrch = oDoc.createSearchDescriptor
                    oSrch = oDoc.createReplaceDescriptor
                    oSrch.setSearchString("naimenovan")
                    oSrch.setReplaceString(.Fields("PRINTER_NAME_1"))
                    Debug.Print(oDoc.replaceAll(oSrch))

                    oSrch.setSearchString("kab otd")
                    oSrch.setReplaceString(.Fields("FILIAL").Value & "\" & .Fields("mesto").Value & "\" & .Fields("kabn").Value)
                    Debug.Print(oDoc.replaceAll(oSrch))

                    oSrch.setSearchString("otvet")
                    oSrch.setReplaceString(.Fields("OTvetstvennyj"))
                    Debug.Print(oDoc.replaceAll(oSrch))

                    oSrch.setSearchString("tel")
                    oSrch.setReplaceString(.Fields("TELEPHONE"))
                    Debug.Print(oDoc.replaceAll(oSrch))

                    oSrch.setSearchString("serial")
                    oSrch.setReplaceString(.Fields("PRINTER_SN_1"))
                    Debug.Print(oDoc.replaceAll(oSrch))

                    oSrch.setSearchString("invent")
                    oSrch.setReplaceString(.Fields("INV_NO_PRINTER"))
                    Debug.Print(oDoc.replaceAll(oSrch))

                    oSrch.setSearchString("format")
                    oSrch.setReplaceString(.Fields("port_1"))
                    Debug.Print(oDoc.replaceAll(oSrch))

                End With


                rs.Close()
                rs = Nothing


            Case Else

                Dim Wrd As Word.Application
                Dim WrdDc As Object
                Wrd = New Word.Application

                WrdDc = Wrd.Documents.Open(tipot, , False)  'Application.
                WrdDc.Application.Visible = True

                Wrd.Selection.Find.ClearFormatting()
                Wrd.Selection.Find.Replacement.ClearFormatting()
                'Номер

                Dim rs As ADODB.Recordset
                sSQL = "SELECT * FROM kompy WHERE id = " & sSID
                rs = New ADODB.Recordset
                rs.Open(sSQL, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

                Dim a1, a2, a3, a4, a5, a6, a7 As String
                With rs

                    a1 = .Fields("PRINTER_NAME_1").Value
                    a2 = .Fields("FILIAL").Value & "\" & .Fields("mesto").Value & "\" & .Fields("kabn").Value
                    a3 = .Fields("OTvetstvennyj").Value
                    a4 = .Fields("TELEPHONE").Value
                    a5 = .Fields("PRINTER_SN_1").Value
                    a6 = .Fields("INV_NO_PRINTER").Value
                    a7 = .Fields("port_1").Value

                End With

                rs.Close()
                rs = Nothing


                With Wrd.Selection.Find
                    .Text = "naimenovan"
                    .Replacement.Text = a1
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "kab otd"
                    .Replacement.Text = a2
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "otvet"
                    .Replacement.Text = a3
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "tel"
                    .Replacement.Text = a4
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "serial"
                    .Replacement.Text = a5
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "invent"
                    .Replacement.Text = a6
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "Format"
                    .Replacement.Text = a7
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)
                WrdDc = Nothing
                Wrd = Nothing
        End Select



    End Sub

    Public Sub PASSWORD_NET(ByVal sSID As String)

        Dim tipot, SIS, sSQL As String

        Dim CONFIGURE As ADODB.Recordset

        CONFIGURE = New ADODB.Recordset
        CONFIGURE.Open("SELECT * FROM CONFIGURE", DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
        With CONFIGURE
            SIS = .Fields("sisadm").Value
        End With
        CONFIGURE.Close()
        CONFIGURE = Nothing

        tipot = Directory.GetParent(Application.ExecutablePath).ToString & "\blanks\pswgl6.dot"


        Select Case sOfficePACK

            Case "OpenOffice.org"

                Dim oSM As Object            'Root object for accessing OpenOffice FROM VB
                Dim oDesk, oDoc As Object 'First objects FROM the API
                Dim arg(-1) As Object             'Ignore it for the moment !

                oSM = CreateObject("com.sun.star.ServiceManager")


                'Create the first and most important service
                oDesk = oSM.createInstance("com.sun.star.frame.Desktop")
                'Create a new doc
                oDoc = oDesk.loadComponentFROMURL("file:///" & tipot, "_blank", 0, arg)
                ' jon code
                Dim objText As Object, objCursor As Object
                objText = oDoc.GetText
                objCursor = objText.createTextCursor

                ' replace all
                Dim oSrch As Object

                Dim rs As ADODB.Recordset
                sSQL = "SELECT * FROM kompy WHERE id = " & sSID
                rs = New ADODB.Recordset
                rs.Open(sSQL, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)


                With rs
                    'Set oSrch = oDoc.createSearchDescriptor
                    oSrch = oDoc.createReplaceDescriptor
                    oSrch.setSearchString("naimenovan")
                    oSrch.setReplaceString(.Fields("PRINTER_SN_1"))
                    Debug.Print(oDoc.replaceAll(oSrch))

                    oSrch.setSearchString("proizv")
                    oSrch.setReplaceString(.Fields("PRINTER_PROIZV_1"))
                    Debug.Print(oDoc.replaceAll(oSrch))

                    oSrch.setSearchString("porti")
                    oSrch.setReplaceString(.Fields("PRINTER_SN_2"))
                    Debug.Print(oDoc.replaceAll(oSrch))

                    oSrch.setSearchString("ipaddr")
                    oSrch.setReplaceString(.Fields("PRINTER_NAME_2"))
                    Debug.Print(oDoc.replaceAll(oSrch))

                    oSrch.setSearchString("tip")
                    oSrch.setReplaceString(.Fields("PRINTER_NAME_1"))
                    Debug.Print(oDoc.replaceAll(oSrch))

                    oSrch.setSearchString("serial")
                    oSrch.setReplaceString(.Fields("port_1"))
                    Debug.Print(oDoc.replaceAll(oSrch))

                    'proizv
                    oSrch.setSearchString("_kab\otd")
                    oSrch.setReplaceString(.Fields("FILIAL").Value & "\" & .Fields("mesto").Value & "\" & .Fields("kabn").Value)
                    Debug.Print(oDoc.replaceAll(oSrch))

                    oSrch.setSearchString("otvet")
                    oSrch.setReplaceString(.Fields("TELEPHONE"))
                    Debug.Print(oDoc.replaceAll(oSrch))

                    oSrch.setSearchString("tel")
                    oSrch.setReplaceString("")
                    Debug.Print(oDoc.replaceAll(oSrch))

                    oSrch.setSearchString("invent")
                    oSrch.setReplaceString(.Fields("PRINTER_PROIZV_3"))
                    Debug.Print(oDoc.replaceAll(oSrch))

                End With

                rs.Close()
                rs = Nothing

            Case Else

                Dim Wrd As Word.Application
                Dim WrdDc As Object
                Wrd = New Word.Application

                WrdDc = Wrd.Documents.Open(tipot, , False)  'Application.
                WrdDc.Application.Visible = True

                Wrd.Selection.Find.ClearFormatting()
                Wrd.Selection.Find.Replacement.ClearFormatting()
                'Номер

                Dim rs As ADODB.Recordset
                sSQL = "SELECT * FROM kompy WHERE id = " & sSID
                rs = New ADODB.Recordset
                rs.Open(sSQL, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

                Dim a1, a2, a3, a4, a5, a6, a7, a8, a9 As String
                With rs

                    a1 = .Fields("PRINTER_SN_1").Value
                    a2 = .Fields("FILIAL").Value & "\" & .Fields("mesto").Value & "\" & .Fields("kabn").Value
                    a3 = .Fields("PRINTER_PROIZV_1").Value
                    a4 = .Fields("PRINTER_SN_2").Value
                    a5 = .Fields("PRINTER_NAME_2").Value
                    a6 = .Fields("PRINTER_NAME_1").Value

                    a7 = .Fields("port_1").Value
                    a8 = .Fields("TELEPHONE").Value
                    a9 = .Fields("PRINTER_PROIZV_3").Value


                End With

                rs.Close()
                rs = Nothing

                With Wrd.Selection.Find
                    .Text = "naimenovan"
                    .Replacement.Text = a1
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "_kab\otd"
                    .Replacement.Text = a2
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "proizv"
                    .Replacement.Text = a3
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "tel"
                    .Replacement.Text = ""
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "serial"
                    .Replacement.Text = a7
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "porti"
                    .Replacement.Text = a4
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "ipaddr"
                    .Replacement.Text = a5
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "tip"
                    .Replacement.Text = a6
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "otvet"
                    .Replacement.Text = a8
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)


                With Wrd.Selection.Find
                    .Text = "invent"
                    .Replacement.Text = a9
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)


                WrdDc = Nothing
                Wrd = Nothing

        End Select



    End Sub

    Public Sub PASSWORD_OTH(ByVal sSID As String, ByVal sTEH As String)
        Dim tipot, SIS, sSQL As String

        Dim CONFIGURE As ADODB.Recordset


        Select Case sTEH

            Case "SCANER"

                tipot = Directory.GetParent(Application.ExecutablePath).ToString & "\blanks\pswgl2.dot"

            Case Else

                'tipot = Directory.GetParent(Application.ExecutablePath).ToString & "\blanks\pswgl5.dot"

        End Select



        CONFIGURE = New ADODB.Recordset
        CONFIGURE.Open("SELECT * FROM CONFIGURE", DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
        With CONFIGURE
            SIS = .Fields("sisadm").Value
        End With
        CONFIGURE.Close()
        CONFIGURE = Nothing

        Select Case sOfficePACK

            Case "OpenOffice.org"

                Dim oSM As Object            'Root object for accessing OpenOffice FROM VB
                Dim oDesk, oDoc As Object 'First objects FROM the API
                Dim arg(-1) As Object             'Ignore it for the moment !

                oSM = CreateObject("com.sun.star.ServiceManager")


                'Create the first and most important service
                oDesk = oSM.createInstance("com.sun.star.frame.Desktop")
                'Create a new doc
                oDoc = oDesk.loadComponentFROMURL("file:///" & tipot, "_blank", 0, arg)
                ' jon code
                Dim objText As Object, objCursor As Object
                objText = oDoc.GetText
                objCursor = objText.createTextCursor

                ' replace all
                Dim oSrch As Object

                Dim rs As ADODB.Recordset
                sSQL = "SELECT * FROM kompy WHERE id = " & sSID
                rs = New ADODB.Recordset
                rs.Open(sSQL, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)


                With rs
                    'Set oSrch = oDoc.createSearchDescriptor
                    oSrch = oDoc.createReplaceDescriptor
                    oSrch.setSearchString("naimenovan")
                    oSrch.setReplaceString(.Fields("PRINTER_NAME_1"))
                    Debug.Print(oDoc.replaceAll(oSrch))

                    oSrch.setSearchString("kab_otd")
                    oSrch.setReplaceString(.Fields("FILIAL").Value & "\" & .Fields("mesto").Value & "\" & .Fields("kabn").Value)
                    Debug.Print(oDoc.replaceAll(oSrch))

                    oSrch.setSearchString("otvet")
                    oSrch.setReplaceString(.Fields("OTvetstvennyj"))
                    Debug.Print(oDoc.replaceAll(oSrch))

                    oSrch.setSearchString("tel")
                    oSrch.setReplaceString(.Fields("TELEPHONE"))
                    Debug.Print(oDoc.replaceAll(oSrch))

                    oSrch.setSearchString("ser")
                    oSrch.setReplaceString(.Fields("PRINTER_SN_1"))
                    Debug.Print(oDoc.replaceAll(oSrch))

                    oSrch.setSearchString("INN")
                    oSrch.setReplaceString(.Fields("INV_NO_PRINTER"))
                    Debug.Print(oDoc.replaceAll(oSrch))


                End With

                rs.Close()
                rs = Nothing

            Case Else

                Dim Wrd As Word.Application
                Dim WrdDc As Object
                Wrd = New Word.Application

                WrdDc = Wrd.Documents.Open(tipot, , False)  'Application.
                WrdDc.Application.Visible = True

                Wrd.Selection.Find.ClearFormatting()
                Wrd.Selection.Find.Replacement.ClearFormatting()
                'Номер

                Dim rs As ADODB.Recordset
                sSQL = "SELECT * FROM kompy WHERE id = " & sSID
                rs = New ADODB.Recordset
                rs.Open(sSQL, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

                Dim a1, a2, a3, a4, a5, a6 As String
                With rs

                    a1 = .Fields("PRINTER_NAME_1").Value
                    a2 = .Fields("FILIAL").Value & "\" & .Fields("mesto").Value & "\" & .Fields("kabn").Value
                    a3 = .Fields("OTvetstvennyj").Value
                    a4 = .Fields("TELEPHONE").Value
                    a5 = .Fields("PRINTER_SN_1").Value
                    a6 = .Fields("INV_NO_PRINTER").Value


                End With

                rs.Close()
                rs = Nothing


                With Wrd.Selection.Find
                    .Text = "naimenovan"
                    .Replacement.Text = a1
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "kab_otd"
                    .Replacement.Text = a2
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "otvet"
                    .Replacement.Text = a3
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "tel"
                    .Replacement.Text = a4
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "ser"
                    .Replacement.Text = a5
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "INN"
                    .Replacement.Text = a6
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                WrdDc = Nothing
                Wrd = Nothing

        End Select

    End Sub

    Public Sub blanks_my_o_ZKP()
        On Error GoTo err_
        Dim LNGIniFile As New IniFile(sLANGPATH)

        Dim tipot As String

        Dim sSQL, sSQL1, SISADM, Organ As String

        tipot = Directory.GetParent(Application.ExecutablePath).ToString & "\blanks\cr_zak.doc"

        If Len(tipot) = 0 Then Exit Sub
        On Error Resume Next


        Dim intj As Integer


        Dim rs As ADODB.Recordset 'Объявляем рекордсет
        Dim Count As String

        rs = New ADODB.Recordset
        rs.Open("Select COUNT(*) as tot_num FROM kompy where OTvetstvennyj='" & frmReports.cmbOTV.Text & "'", DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

        With rs
            Count = .Fields("tot_num").Value
        End With

        rs.Close()
        rs = Nothing
        If Count = 0 Then Exit Sub

        Dim iA, iB, iC, iD, iE As String


        Dim rs1 As ADODB.Recordset
        sSQL1 = "SELECT * FROM CONFIGURE"
        rs1 = New ADODB.Recordset
        rs1.Open(sSQL1, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

        With rs1
            Organ = .Fields("org").Value
            SISADM$ = .Fields("SISADM").Value
        End With
        rs1.Close()
        rs1 = Nothing


        Select Case sOfficePACK

            Case "OpenOffice.org"


                Dim oSM As Object            'Root object for accessing OpenOffice FROM VB
                Dim oDesk, oDoc As Object 'First objects FROM the API
                Dim arg(-1) As Object             'Ignore it for the moment !
                Dim mmerge As Object
                'Instanciate OOo : this line is mandatory with VB for OOo API

                Dim objCoreReflection As Object ' objects from OOo API 
                'Dim objText As Object, objCursor As Object

                Dim objTable As Object ' objects from OOo API 
                Dim objRows, objRow, objColumns As Object

                oSM = CreateObject("com.sun.star.ServiceManager")


                'Create the first and most important service
                oDesk = oSM.createInstance("com.sun.star.frame.Desktop")
                'Create a new doc
                oDoc = oDesk.loadComponentFROMURL("file:///" & tipot, "_blank", 0, arg)
                ' jon code
                Dim objText As Object, objCursor As Object
                objText = oDoc.GetText
                objCursor = objText.createTextCursor

                ' replace all
                Dim oSrch As Object

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString(LNGIniFile.GetString("MOD_OPENOFFICE", "MSG2", ""))
                oSrch.setReplaceString(Organ)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString(LNGIniFile.GetString("MOD_OPENOFFICE", "MSG5", ""))
                oSrch.setReplaceString(frmReports.cmbOTV.Text)
                Debug.Print(oDoc.replaceAll(oSrch))


                rs = New ADODB.Recordset
                rs.Open("Select * FROM kompy where OTvetstvennyj='" & frmReports.cmbOTV.Text & "'", DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

                With rs
                    iA = .Fields("FILIAL").Value
                    iB = .Fields("mesto").Value
                    iC = .Fields("kabn").Value
                End With

                rs.Close()
                rs = Nothing

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("Branch")
                oSrch.setReplaceString(iA)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("Department")
                oSrch.setReplaceString(iB)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("Office")
                oSrch.setReplaceString(iC)
                Debug.Print(oDoc.replaceAll(oSrch))


                rs = New ADODB.Recordset
                rs.Open("Select * FROM kompy where OTvetstvennyj='" & frmReports.cmbOTV.Text & "'", DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)



                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("TABLE")
                oSrch.setReplaceString("")
                Debug.Print(oDoc.replaceAll(oSrch))


                intj = 1

                objTable = oDoc.createInstance("com.sun.star.text.TextTable")
                objTable.Initialize(Count + 1, 5)
                objText.insertTextContent(objCursor, objTable, False)

                'Ширина полей таблицы
                objColumns = objTable.TableColumnSeparators
                objColumns(0).Position = 500

                objTable.TableColumnSeparators = objColumns


                objRows = objTable.GetRows
                objRow = objRows.getByIndex(0)

                objTable.setPropertyValue("BackTransparent", True)
                objTable.setPropertyValue("BackColor", 16777215)

                'LNGIniFile.GetString("MOD_OPENOFFICE", "MSG38", "")
                intj = 1
                insertIntoCell("A1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG53", ""), objTable)
                insertIntoCell("B1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG54", ""), objTable)
                insertIntoCell("C1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG55", ""), objTable)
                insertIntoCell("D1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG56", ""), objTable)
                insertIntoCell("E1", LNGIniFile.GetString("MOD_OPENOFFICE", "MSG57", ""), objTable)


                'Column.OlePropertySet("optimalWidth", True)

                With rs
                    .MoveFirst()
                    Do While Not .EOF

                        iA = "A" & intj + 1
                        iB = "B" & intj + 1
                        iC = "C" & intj + 1
                        iD = "D" & intj + 1
                        iE = "E" & intj + 1

                        Select Case .Fields("tiptehn").Value

                            Case "PC"

                                insertIntoCell(iA, intj, objTable)
                                insertIntoCell(iB, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG58", "") & " - " & .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("INV_NO_SYSTEM").Value, objTable)
                                insertIntoCell(iD, .Fields("Ser_N_SIS").Value, objTable)
                                insertIntoCell(iE, .Fields("DataVVoda").Value, objTable)

                                intj = intj + 1

                            Case "Printer"

                                insertIntoCell(iA, intj, objTable)
                                insertIntoCell(iB, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG27", "") & " - " & .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("INV_NO_PRINTER").Value, objTable)
                                insertIntoCell(iD, .Fields("PRINTER_SN_1").Value, objTable)
                                insertIntoCell(iE, .Fields("DataVVoda").Value, objTable)



                                intj = intj + 1

                            Case "MFU"

                                insertIntoCell(iA, intj, objTable)
                                insertIntoCell(iB, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG32", "") & " - " & .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("INV_NO_PRINTER").Value, objTable)
                                insertIntoCell(iD, .Fields("PRINTER_SN_1").Value, objTable)
                                insertIntoCell(iE, .Fields("DataVVoda").Value, objTable)

                                intj = intj + 1

                            Case "OT"

                                insertIntoCell(iA, intj, objTable)

                                If Len(.Fields("TIP_COMPA").Value) = 0 Then

                                    insertIntoCell(iB, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG59", "") & " - (" & .Fields("TIP_COMPA").Value & ") " & .Fields("NET_NAME").Value, objTable)

                                Else

                                    insertIntoCell(iB, .Fields("TIP_COMPA").Value & " - " & .Fields("NET_NAME").Value, objTable)

                                End If


                                insertIntoCell(iC, .Fields("INV_NO_PRINTER").Value, objTable)
                                insertIntoCell(iD, .Fields("PRINTER_SN_1").Value, objTable)
                                insertIntoCell(iE, .Fields("DataVVoda").Value, objTable)

                                intj = intj + 1


                            Case "KOpir"

                                insertIntoCell(iA, intj, objTable)
                                insertIntoCell(iB, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG60", "") & " - " & .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("INV_NO_PRINTER").Value, objTable)
                                insertIntoCell(iD, .Fields("PRINTER_SN_1").Value, objTable)
                                insertIntoCell(iE, .Fields("DataVVoda").Value, objTable)
                                intj = intj + 1

                            Case "NET"

                                insertIntoCell(iA, intj, objTable)
                                insertIntoCell(iB, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG61", "") & " - " & .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("PRINTER_PROIZV_3").Value, objTable)
                                insertIntoCell(iD, .Fields("port_1").Value, objTable)
                                insertIntoCell(iE, .Fields("DataVVoda").Value, objTable)

                                intj = intj + 1

                            Case "PHOTO"

                                insertIntoCell(iA, intj, objTable)
                                insertIntoCell(iB, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG62", "") & " - " & .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("INV_NO_PRINTER").Value, objTable)
                                insertIntoCell(iD, .Fields("PRINTER_SN_1").Value, objTable)
                                insertIntoCell(iE, .Fields("DataVVoda").Value, objTable)

                                intj = intj + 1

                            Case "PHONE"

                                insertIntoCell(iA, intj, objTable)
                                insertIntoCell(iB, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG63", "") & " - " & .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("INV_NO_PRINTER").Value, objTable)
                                insertIntoCell(iD, .Fields("PRINTER_SN_1").Value, objTable)
                                insertIntoCell(iE, .Fields("DataVVoda").Value, objTable)

                                intj = intj + 1

                            Case "FAX"
                                insertIntoCell(iA, intj, objTable)
                                insertIntoCell(iB, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG64", "") & " - " & .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("INV_NO_PRINTER").Value, objTable)
                                insertIntoCell(iD, .Fields("PRINTER_SN_1").Value, objTable)
                                insertIntoCell(iE, .Fields("DataVVoda").Value, objTable)

                                intj = intj + 1

                            Case "SCANER"
                                insertIntoCell(iA, intj, objTable)
                                insertIntoCell(iB, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG34", "") & " - " & .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("INV_NO_PRINTER").Value, objTable)
                                insertIntoCell(iD, .Fields("PRINTER_SN_1").Value, objTable)
                                insertIntoCell(iE, .Fields("DataVVoda").Value, objTable)


                                intj = intj + 1

                            Case "ZIP"

                                insertIntoCell(iA, intj, objTable)
                                insertIntoCell(iB, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG65", "") & " - " & .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("INV_NO_PRINTER").Value, objTable)
                                insertIntoCell(iD, .Fields("PRINTER_SN_1").Value, objTable)
                                insertIntoCell(iE, .Fields("DataVVoda").Value, objTable)

                                intj = intj + 1


                            Case "MONITOR"

                                insertIntoCell(iA, intj, objTable)
                                insertIntoCell(iB, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG24", "") & " - " & .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("INV_NO_MONITOR").Value, objTable)
                                insertIntoCell(iD, .Fields("MONITOR_SN").Value, objTable)
                                insertIntoCell(iE, .Fields("DataVVoda").Value, objTable)

                                'DataVVoda
                                intj = intj + 1


                            Case "USB"

                                insertIntoCell(iA, intj, objTable)
                                insertIntoCell(iB, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG37", "") & " - " & .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("INV_NO_MONITOR").Value, objTable)
                                insertIntoCell(iD, .Fields("MONITOR_SN").Value, objTable)
                                insertIntoCell(iE, .Fields("DataVVoda").Value, objTable)

                                'DataVVoda
                                intj = intj + 1

                            Case "SOUND"

                                insertIntoCell(iA, intj, objTable)
                                insertIntoCell(iB, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG25", "") & " - " & .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("INV_NO_MONITOR").Value, objTable)
                                insertIntoCell(iD, .Fields("MONITOR_SN").Value, objTable)
                                insertIntoCell(iE, .Fields("DataVVoda").Value, objTable)

                                'DataVVoda
                                intj = intj + 1

                            Case "IBP"

                                insertIntoCell(iA, intj, objTable)
                                insertIntoCell(iB, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG35", "") & " - " & .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("INV_NO_MONITOR").Value, objTable)
                                insertIntoCell(iD, .Fields("MONITOR_SN").Value, objTable)
                                insertIntoCell(iE, .Fields("DataVVoda").Value, objTable)

                                'DataVVoda
                                intj = intj + 1

                            Case ("FS")

                                insertIntoCell(iA, intj, objTable)
                                insertIntoCell(iB, LNGIniFile.GetString("MOD_OPENOFFICE", "MSG26", "") & " - " & .Fields("NET_NAME").Value, objTable)
                                insertIntoCell(iC, .Fields("INV_NO_MONITOR").Value, objTable)
                                insertIntoCell(iD, .Fields("MONITOR_SN").Value, objTable)
                                insertIntoCell(iE, .Fields("DataVVoda").Value, objTable)

                                'DataVVoda
                                intj = intj + 1

                        End Select

                        .MoveNext()
                        'DoEvents
                    Loop
                End With

                rs.Close()
                rs = Nothing


                objText.insertControlCharacter(objCursor, 0, False)


            Case Else

                Dim Wrd As Word.Application
                Dim WrdDc As Word.Document
                Wrd = New Word.Application

                Dim oDoc As Word.Document
                Dim oTable As Word.Table
                Dim oPara1 As Word.Paragraph, oPara2 As Word.Paragraph
                Dim oPara3 As Word.Paragraph, oPara4 As Word.Paragraph
                Dim oRng As Word.Range
                Dim oShape As Word.InlineShape
                Dim oChart As Object
                Dim Pos As Double

                'oDoc = Wrd.Documents.Add


                WrdDc = Wrd.Documents.Open(tipot, , False)  'Application.
                WrdDc.Application.Visible = True

                Wrd.Selection.Find.ClearFormatting()
                Wrd.Selection.Find.Replacement.ClearFormatting()


                rs = New ADODB.Recordset
                rs.Open("Select * FROM kompy where OTvetstvennyj='" & frmReports.cmbOTV.Text & "'", DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

                With rs
                    iA = .Fields("FILIAL").Value
                    iB = .Fields("mesto").Value
                    iC = .Fields("kabn").Value
                End With

                rs.Close()
                rs = Nothing

                With Wrd.Selection.Find
                    .Text = "Организация"
                    .Replacement.Text = Organ
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "Ответственный"
                    .Replacement.Text = frmReports.cmbOTV.Text
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)


                With Wrd.Selection.Find
                    .Text = "Branch"
                    .Replacement.Text = iA
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "Department"
                    .Replacement.Text = iB
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "Office"
                    .Replacement.Text = iC
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                'rs = New ADODB.Recordset
                'rs.Open("Select * FROM kompy where OTvetstvennyj='" & frmReports.cmbOTV.Text & "'", DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

                With Wrd.Selection.Find
                    .Text = "TABLE"
                    .Replacement.Text = ""
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)




                oTable = WrdDc.Tables.Add(WrdDc.Bookmarks.Item("TABLE1").Range, Count + 1, 5)
                oTable.Range.ParagraphFormat.SpaceAfter = 6

                oTable.Cell(1, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG53", "")
                oTable.Cell(1, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG54", "")
                oTable.Cell(1, 3).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG55", "")
                oTable.Cell(1, 4).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG56", "")
                oTable.Cell(1, 5).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG57", "")


                intj = 2

                rs = New ADODB.Recordset
                rs.Open("Select * FROM kompy where OTvetstvennyj='" & frmReports.cmbOTV.Text & "'", DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

                With rs
                    .MoveFirst()
                    Do While Not .EOF


                        Select Case .Fields("tiptehn").Value



                            Case "PC"

                                oTable.Cell(intj, 1).Range.Text = intj - 1
                                oTable.Cell(intj, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG58", "") & " - " & .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("INV_NO_SYSTEM").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("Ser_N_SIS").Value
                                oTable.Cell(intj, 5).Range.Text = .Fields("DataVVoda").Value

                                intj = intj + 1

                            Case "Printer"

                                oTable.Cell(intj, 1).Range.Text = intj - 1
                                oTable.Cell(intj, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG27", "") & " - " & .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("INV_NO_PRINTER").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("PRINTER_SN_1").Value
                                oTable.Cell(intj, 5).Range.Text = .Fields("DataVVoda").Value



                                intj = intj + 1

                            Case "MFU"

                                oTable.Cell(intj, 1).Range.Text = intj - 1
                                oTable.Cell(intj, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG32", "") & " - " & .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("INV_NO_PRINTER").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("PRINTER_SN_1").Value
                                oTable.Cell(intj, 5).Range.Text = .Fields("DataVVoda").Value

                                intj = intj + 1

                            Case "OT"
                                oTable.Cell(intj, 1).Range.Text = intj - 1

                                If Len(.Fields("TIP_COMPA").Value) = 0 Then

                                    oTable.Cell(intj, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG59", "") & " - (" & .Fields("TIP_COMPA").Value & ") " & .Fields("NET_NAME").Value
                                Else
                                    oTable.Cell(intj, 2).Range.Text = .Fields("TIP_COMPA").Value & " - " & .Fields("NET_NAME").Value

                                End If


                                oTable.Cell(intj, 3).Range.Text = .Fields("INV_NO_PRINTER").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("PRINTER_SN_1").Value
                                oTable.Cell(intj, 5).Range.Text = .Fields("DataVVoda").Value

                                intj = intj + 1


                            Case "KOpir"

                                oTable.Cell(intj, 1).Range.Text = intj - 1
                                oTable.Cell(intj, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG60", "") & " - " & .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("INV_NO_PRINTER").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("PRINTER_SN_1").Value
                                oTable.Cell(intj, 5).Range.Text = .Fields("DataVVoda").Value
                                intj = intj + 1

                            Case "NET"

                                oTable.Cell(intj, 1).Range.Text = intj - 1
                                oTable.Cell(intj, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG61", "") & " - " & .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_PROIZV_3").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("port_1").Value
                                oTable.Cell(intj, 5).Range.Text = .Fields("DataVVoda").Value

                                intj = intj + 1

                            Case "PHOTO"

                                oTable.Cell(intj, 1).Range.Text = intj - 1
                                oTable.Cell(intj, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG62", "") & " - " & .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("INV_NO_PRINTER").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("PRINTER_SN_1").Value
                                oTable.Cell(intj, 5).Range.Text = .Fields("DataVVoda").Value

                                intj = intj + 1

                            Case "PHONE"

                                oTable.Cell(intj, 1).Range.Text = intj - 1
                                oTable.Cell(intj, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG63", "") & " - " & .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("INV_NO_PRINTER").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("PRINTER_SN_1").Value
                                oTable.Cell(intj, 5).Range.Text = .Fields("DataVVoda").Value

                                intj = intj + 1

                            Case "FAX"
                                oTable.Cell(intj, 1).Range.Text = intj - 1
                                oTable.Cell(intj, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG64", "") & " - " & .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("INV_NO_PRINTER").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("PRINTER_SN_1").Value
                                oTable.Cell(intj, 5).Range.Text = .Fields("DataVVoda").Value

                                intj = intj + 1

                            Case "SCANER"
                                oTable.Cell(intj, 1).Range.Text = intj - 1
                                oTable.Cell(intj, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG34", "") & " - " & .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("INV_NO_PRINTER").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("PRINTER_SN_1").Value
                                oTable.Cell(intj, 5).Range.Text = .Fields("DataVVoda").Value


                                intj = intj + 1

                            Case "ZIP"

                                oTable.Cell(intj, 1).Range.Text = intj - 1
                                oTable.Cell(intj, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG65", "") & " - " & .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("INV_NO_PRINTER").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("PRINTER_SN_1").Value
                                oTable.Cell(intj, 5).Range.Text = .Fields("DataVVoda").Value

                                intj = intj + 1


                            Case "MONITOR"

                                oTable.Cell(intj, 1).Range.Text = intj - 1
                                oTable.Cell(intj, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG24", "") & " - " & .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("INV_NO_MONITOR").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("MONITOR_SN").Value
                                oTable.Cell(intj, 5).Range.Text = .Fields("DataVVoda").Value

                                'DataVVoda
                                intj = intj + 1

                            Case "FS"

                                oTable.Cell(intj, 1).Range.Text = intj - 1
                                oTable.Cell(intj, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG26", "") & " - " & .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("INV_NO_MONITOR").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("MONITOR_SN").Value
                                oTable.Cell(intj, 5).Range.Text = .Fields("DataVVoda").Value

                                'DataVVoda
                                intj = intj + 1

                            Case "IBP"

                                oTable.Cell(intj, 1).Range.Text = intj - 1
                                oTable.Cell(intj, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG35", "") & " - " & .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("INV_NO_MONITOR").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("MONITOR_SN").Value
                                oTable.Cell(intj, 5).Range.Text = .Fields("DataVVoda").Value

                                'DataVVoda
                                intj = intj + 1

                            Case "SOUND"

                                oTable.Cell(intj, 1).Range.Text = intj - 1
                                oTable.Cell(intj, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG25", "") & " - " & .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("INV_NO_MONITOR").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("MONITOR_SN").Value
                                oTable.Cell(intj, 5).Range.Text = .Fields("DataVVoda").Value

                                'DataVVoda
                                intj = intj + 1

                            Case "USB"

                                oTable.Cell(intj, 1).Range.Text = intj - 1
                                oTable.Cell(intj, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG37", "") & " - " & .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("INV_NO_MONITOR").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("MONITOR_SN").Value
                                oTable.Cell(intj, 5).Range.Text = .Fields("DataVVoda").Value

                                'DataVVoda
                                intj = intj + 1

                        End Select


                        .MoveNext()
                        'DoEvents
                    Loop
                End With

                rs.Close()
                rs = Nothing

                oTable.Columns.Item(1).Width = Wrd.InchesToPoints(0.5)   'Change width of columns 1 & 2
                oTable.Columns.Item(2).Width = Wrd.InchesToPoints(2.5)

        End Select



        Exit Sub
err_:

        MsgBox(Err.Description)
    End Sub

    Public Sub WRD_SEND_PK(ByVal sID As String)
        On Error Resume Next
        Dim LNGIniFile As New IniFile(sLANGPATH)

        Dim rs As ADODB.Recordset
        Dim rs1 As ADODB.Recordset
        Dim sSQL, sSQL1, scN, cOTV As String

        Dim iA As String
        Dim iB As String
        Dim iC As String
        Dim iD As String
        Dim iE As String
        Dim uname As String

        Dim oWord As Word.Application
        Dim oDoc As Word.Document
        Dim oTable As Word.Table
        Dim oPara1 As Word.Paragraph, oPara2 As Word.Paragraph
        Dim oPara3 As Word.Paragraph, oPara4 As Word.Paragraph
        Dim oRng As Word.Range
        Dim oShape As Word.InlineShape
        Dim oChart As Object
        Dim Pos As Double



        Call COUNT_FIELDS(sID)

        sSQL = "SELECT * FROM kompy WHERE TipTehn='PC' AND id=" & sID

        Dim CONFIGURE As ADODB.Recordset
        CONFIGURE = New ADODB.Recordset
        CONFIGURE.Open("SELECT * FROM CONFIGURE", DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly)

        With CONFIGURE
            If Not IsDBNull(.Fields("ORG").Value) Then uname = .Fields("ORG").Value
        End With
        CONFIGURE.Close()
        CONFIGURE = Nothing



        rs = New ADODB.Recordset
        rs.Open(sSQL, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
        Dim intj As Integer

        With rs

            'Start Word and open the document template.
            oWord = CreateObject("Word.Application")
            oWord.Visible = True
            oDoc = oWord.Documents.Add

            'Insert a paragraph at the beginning of the document.
            oPara1 = oDoc.Content.Paragraphs.Add
            oPara1.Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG1", "") & " " & sID
            oPara1.Range.Font.Bold = True
            oPara1.Format.SpaceAfter = 24    '24 pt spacing after paragraph.
            oPara1.Range.InsertParagraphAfter()

            oTable = oDoc.Tables.Add(oDoc.Bookmarks.Item("\endofdoc").Range, 7, 2)
            oTable.Range.ParagraphFormat.SpaceAfter = 6

            cOTV = .Fields("OTvetstvennyj").Value

            oTable.Cell(1, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG2", "")
            oTable.Cell(2, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG3", "")
            oTable.Cell(3, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG4", "")
            oTable.Cell(4, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG5", "")
            oTable.Cell(5, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG6", "")
            oTable.Cell(6, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG7", "")
            oTable.Cell(7, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG8", "")

            oTable.Cell(1, 2).Range.Text = uname
            oTable.Cell(2, 2).Range.Text = .Fields("filial").Value & "\" & .Fields("mesto").Value & "\" & .Fields("kabn").Value
            oTable.Cell(3, 2).Range.Text = .Fields("TIP_COMPA").Value
            oTable.Cell(4, 2).Range.Text = .Fields("OTvetstvennyj").Value
            oTable.Cell(5, 2).Range.Text = .Fields("NET_NAME").Value
            oTable.Cell(6, 2).Range.Text = .Fields("PSEVDONIM").Value
            oTable.Cell(7, 2).Range.Text = .Fields("INV_NO_SYSTEM").Value


            oTable.Rows.Item(1).Range.Font.Bold = True
            oTable.Rows.Item(1).Range.Font.Italic = False

            oPara1 = oDoc.Content.Paragraphs.Add
            oPara1.Range.Text = ""
            oPara1.Range.Font.Bold = True
            oPara1.Format.SpaceAfter = 24    '24 pt spacing after paragraph.
            oPara1.Range.InsertParagraphAfter()

            'iCOUNTFIELLDS + 1

            oTable = oDoc.Tables.Add(oDoc.Bookmarks.Item("\endofdoc").Range, iCOUNTFIELLDS + 1, 3)
            oTable.Range.ParagraphFormat.SpaceAfter = 6

            oTable.Cell(1, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG9", "")
            oTable.Cell(1, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG10", "")
            oTable.Cell(1, 3).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG11", "")

            oTable.Cell(2, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG12", "")
            oTable.Cell(2, 2).Range.Text = .Fields("Mb").Value & ", " & "SN: " & .Fields("Mb_Id").Value
            oTable.Cell(2, 3).Range.Text = .Fields("Mb_Proizvod").Value

            oTable.Cell(3, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG13", "")
            oTable.Cell(3, 2).Range.Text = .Fields("CPU1").Value & ", " & .Fields("CPUmhz1").Value
            oTable.Cell(3, 3).Range.Text = .Fields("CPUProizv1").Value

            intj = 4

            oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG13", "")
            oTable.Cell(intj, 2).Range.Text = .Fields("CPU1").Value & ", " & .Fields("CPUmhz1").Value
            oTable.Cell(intj, 3).Range.Text = .Fields("CPUProizv1").Value


            If Len(.Fields("CPU2").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG13" & " 2", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("CPU2").Value & ", " & .Fields("CPUmhz2").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("CPUProizv2").Value

                intj = intj + 1

            End If

            If Len(.Fields("CPU3").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG13" & " 3", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("CPU3").Value & ", " & .Fields("CPUmhz3").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("CPUProizv3").Value

                intj = intj + 1

            End If

            If Len(.Fields("CPU4").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG13" & " 4", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("CPU4").Value & ", " & .Fields("CPUmhz4").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("CPUProizv4").Value

                intj = intj + 1

            End If

            If Len(.Fields("RAM_1").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG14" & "", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("RAM_1").Value & ", " & .Fields("RAM_SN_1").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("RAM_PROIZV_1").Value

                intj = intj + 1

            End If

            If Len(.Fields("RAM_2").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG14" & " 2", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("RAM_2").Value & ", " & "SN: " & .Fields("RAM_SN_2").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("RAM_PROIZV_2").Value

                intj = intj + 1

            End If

            If Len(.Fields("RAM_3").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG14" & " 3", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("RAM_3").Value & ", " & "SN: " & .Fields("RAM_SN_3").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("RAM_PROIZV_3").Value

                intj = intj + 1

            End If

            If Len(.Fields("RAM_4").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG14" & " 4", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("RAM_4").Value & ", " & "SN: " & .Fields("RAM_SN_4").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("RAM_PROIZV_4").Value

                intj = intj + 1

            End If

            If Len(.Fields("HDD_Name_1").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG15", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("HDD_Name_1").Value & ", " & .Fields("HDD_OB_1").Value & ", SN: " & .Fields("HDD_SN_1").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("HDD_PROIZV_1").Value

                intj = intj + 1

            End If

            If Len(.Fields("HDD_Name_2").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG15", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("HDD_Name_2").Value & ", " & .Fields("HDD_OB_2").Value & ", SN: " & .Fields("HDD_SN_2").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("HDD_PROIZV_2").Value

                intj = intj + 1

            End If

            If Len(.Fields("HDD_Name_3").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG15", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("HDD_Name_3").Value & ", " & .Fields("HDD_OB_3").Value & ", SN: " & .Fields("HDD_SN_3").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("HDD_PROIZV_3").Value

                intj = intj + 1

            End If

            If Len(.Fields("HDD_Name_4").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG15", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("HDD_Name_4").Value & ", " & .Fields("HDD_OB_4").Value & ", SN: " & .Fields("HDD_SN_4").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("HDD_PROIZV_4").Value

                intj = intj + 1

            End If

            If Len(.Fields("FDD_NAME").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG16", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("FDD_NAME").Value & ", SN: " & .Fields("FDD_SN").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("FDD_PROIZV").Value

                intj = intj + 1

            End If

            If Len(.Fields("SVGA_NAME").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG17", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("SVGA_NAME").Value & ", SN: " & .Fields("SVGA_SN").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("SVGA_PROIZV").Value

                intj = intj + 1

            End If

            If Len(.Fields("SOUND_NAME").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG18", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("SOUND_NAME").Value & ", SN: " & .Fields("SOUND_SN").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("SOUND_PROIZV").Value

                intj = intj + 1

            End If

            If Len(.Fields("CD_NAME").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG19", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("CD_NAME").Value & ", SN: " & .Fields("CD_SN").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("CD_PROIZV").Value

                intj = intj + 1

            End If

            If Len(.Fields("CDRW_NAME").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG19", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("CDRW_NAME").Value & ", SN: " & .Fields("CDRW_SN").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("CDRW_PROIZV").Value

                intj = intj + 1

            End If

            If Len(.Fields("DVD_NAME").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG19", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("DVD_NAME").Value & ", SN: " & .Fields("DVD_SN").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("DVD_PROIZV").Value

                intj = intj + 1

            End If

            If Len(.Fields("NET_NAME_1").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG20", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME_1").Value & ", MAC: " & .Fields("NET_MAC_1").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("NET_PROIZV_1").Value

                intj = intj + 1

            End If

            If Len(.Fields("NET_NAME_2").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG20", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME_2").Value & ", MAC: " & .Fields("NET_MAC_2").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("NET_PROIZV_2").Value

                intj = intj + 1

            End If

            If Len(.Fields("BLOCK").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG21", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("BLOCK").Value & ", SN: " & .Fields("SN_BLOCK").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("PROIZV_BLOCK").Value

                intj = intj + 1

            End If

            If Len(.Fields("KEYBOARD_NAME").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG22", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("KEYBOARD_NAME").Value & ", SN: " & .Fields("KEYBOARD_SN").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("KEYBOARD_PROIZV").Value

                intj = intj + 1

            End If

            If Len(.Fields("MOUSE_NAME").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG23", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("MOUSE_NAME").Value & ", SN: " & .Fields("MOUSE_SN").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("MOUSE_PROIZV").Value

                intj = intj + 1

            End If

            If Len(.Fields("MONITOR_NAME").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG24", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("MONITOR_NAME").Value & ", SN: " & .Fields("MONITOR_SN").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("MONITOR_PROIZV").Value

                intj = intj + 1

            End If

            If Len(.Fields("MONITOR_NAME2").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG24", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("MONITOR_NAME2").Value & ", SN: " & .Fields("MONITOR_SN2").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("MONITOR_PROIZV2").Value

                intj = intj + 1

            End If

            If Len(.Fields("AS_NAME").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG25", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("AS_NAME").Value & ", SN: " & .Fields("AS_SN").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("AS_PROIZV").Value

                intj = intj + 1

            End If


            If Len(.Fields("FILTR_NAME").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG26", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("FILTR_NAME").Value & ", SN: " & .Fields("FILTR_SN").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("FILTR_PROIZV").Value

                intj = intj + 1

            End If



            If Len(.Fields("PRINTER_NAME_1").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG27", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("PRINTER_NAME_1").Value & ", SN: " & .Fields("PRINTER_SN_1").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_PROIZV_1").Value

                intj = intj + 1

            End If

            If Len(.Fields("PRINTER_NAME_2").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG27", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("PRINTER_NAME_2").Value & ", SN: " & .Fields("PRINTER_SN_2").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_PROIZV_2").Value

                intj = intj + 1

            End If

            If Len(.Fields("PRINTER_NAME_3").Value) = 0 Then
            Else
                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG27", "")
                oTable.Cell(intj, 2).Range.Text = .Fields("PRINTER_NAME_3").Value & ", SN: " & .Fields("PRINTER_SN_3").Value
                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_PROIZV_3").Value

                intj = intj + 1

            End If


        End With

        rs.Close()
        rs = Nothing


        'В составе устройства

        sSQL1 = "SELECT count(*) as t_n FROM kompy WHERE PCL=" & sID

        rs1 = New ADODB.Recordset
        rs1.Open(sSQL1, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

        Dim GIST As String

        With rs1

            GIST = .Fields("t_n").Value

        End With

        rs1.Close()
        rs1 = Nothing

        If GIST = 0 Then

        Else
            oPara1 = oDoc.Content.Paragraphs.Add
            oPara1.Range.Text = ""
            oPara1.Range.Font.Bold = True
            oPara1.Format.SpaceAfter = 12    '24 pt spacing after paragraph.
            oPara1.Range.InsertParagraphAfter()

            oPara1 = oDoc.Content.Paragraphs.Add
            oPara1.Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG28", "")
            oPara1.Range.Font.Bold = True
            oPara1.Format.SpaceAfter = 12    '24 pt spacing after paragraph.
            oPara1.Range.InsertParagraphAfter()

            'iCOUNTFIELLDS + 1

            oTable = oDoc.Tables.Add(oDoc.Bookmarks.Item("\endofdoc").Range, GIST + 1, 3)
            oTable.Range.ParagraphFormat.SpaceAfter = 6

            oTable.Cell(1, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG29", "")
            oTable.Cell(1, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG10", "")
            oTable.Cell(1, 3).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG31", "")

            sSQL1 = "SELECT * FROM kompy WHERE PCL=" & sID
            rs1 = New ADODB.Recordset
            rs1.Open(sSQL1, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

            intj = 2

            With rs1
                If .RecordCount <> 0 Then
                    .MoveFirst()
                    Do While Not .EOF


                        Select Case .Fields("tiptehn").Value

                            Case "Printer"
                                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG27", "")
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_SN_1").Value

                            Case "MFU"
                                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG32", "")
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_SN_1").Value

                            Case "MONITOR"
                                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG24", "")
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("MONITOR_SN").Value

                            Case "OT"
                                oTable.Cell(intj, 1).Range.Text = .Fields("TIP_COMPA").Value
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_SN_1").Value


                            Case "SCANER"
                                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG34", "")
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_SN_1").Value

                            Case "USB"
                                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG37", "")
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_SN_1").Value

                            Case "IBP"
                                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG35", "")
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_SN_1").Value

                            Case "FS"
                                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG26", "")
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_SN_1").Value

                            Case "SOUND"
                                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG25", "")
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_SN_1").Value


                            Case "MOUSE"

                                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG23", "")
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_SN_1").Value

                            Case "KEYB"

                                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG22", "")
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_SN_1").Value


                        End Select



                        intj = intj + 1

                        .MoveNext()
                    Loop
                End If
            End With
            rs1.Close()
            rs1 = Nothing

        End If

        'Установленное ПО

        sSQL1 = "SELECT count(*) as t_n FROM SOFT_INSTALL WHERE id_comp=" & sID

        rs1 = New ADODB.Recordset
        rs1.Open(sSQL1, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

        With rs1

            GIST = .Fields("t_n").Value

        End With

        rs1.Close()
        rs1 = Nothing

        If GIST = 0 Then
        Else

            oPara1 = oDoc.Content.Paragraphs.Add
            oPara1.Range.Text = ""
            oPara1.Range.Font.Bold = True
            oPara1.Format.SpaceAfter = 12    '24 pt spacing after paragraph.
            oPara1.Range.InsertParagraphAfter()

            oPara1 = oDoc.Content.Paragraphs.Add
            oPara1.Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG38", "")
            oPara1.Range.Font.Bold = True
            oPara1.Format.SpaceAfter = 12    '24 pt spacing after paragraph.
            oPara1.Range.InsertParagraphAfter()


            'iCOUNTFIELLDS + 1

            oTable = oDoc.Tables.Add(oDoc.Bookmarks.Item("\endofdoc").Range, GIST + 1, 1)
            oTable.Range.ParagraphFormat.SpaceAfter = 6

            oTable.Cell(1, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG38", "")

            intj = 2

            sSQL1 = "SELECT * FROM SOFT_INSTALL WHERE id_comp=" & sID
            rs1 = New ADODB.Recordset
            rs1.Open(sSQL1, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
            With rs1
                If .RecordCount <> 0 Then
                    .MoveFirst()
                    Do While Not .EOF

                        oTable.Cell(intj, 1).Range.Text = .Fields("Soft").Value
                        intj = intj + 1

                        .MoveNext()
                    Loop
                End If
            End With

            rs1.Close()
            rs1 = Nothing


        End If

        '#####################################################################################
        'ПЕРЕМЕЩЕНИЕ ТЕХНИКИ


        Dim rscount As ADODB.Recordset 'Объявляем рекордсет
        rscount = New ADODB.Recordset
        rscount.Open("SELECT COUNT(*) AS total_number FROM dvig WHERE id_comp=" & sID, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

        With rscount
            GIST = .Fields("total_number").Value
        End With

        rscount.Close()
        rscount = Nothing


        If GIST > 0 Then

            oPara1 = oDoc.Content.Paragraphs.Add
            oPara1.Range.Text = ""
            oPara1.Range.Font.Bold = True
            oPara1.Format.SpaceAfter = 12    '24 pt spacing after paragraph.
            oPara1.Range.InsertParagraphAfter()

            oPara1 = oDoc.Content.Paragraphs.Add
            oPara1.Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG39", "")
            oPara1.Range.Font.Bold = True
            oPara1.Format.SpaceAfter = 12    '24 pt spacing after paragraph.
            oPara1.Range.InsertParagraphAfter()

            oTable = oDoc.Tables.Add(oDoc.Bookmarks.Item("\endofdoc").Range, GIST + 1, 4)
            oTable.Range.ParagraphFormat.SpaceAfter = 6

            oTable.Cell(1, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG40", "")
            oTable.Cell(1, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG41", "")
            oTable.Cell(1, 3).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG42", "")
            oTable.Cell(1, 4).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG43", "")

            intj = 2
            rscount = New ADODB.Recordset
            rscount.Open("SELECT * FROM dvig WHERE id_comp=" & sID & " ORDER by Data", DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

            intj = 2
            With rscount
                .MoveFirst()
                Do While Not .EOF = True

                    oTable.Cell(intj, 1).Range.Text = .Fields("oldMesto").Value
                    oTable.Cell(intj, 2).Range.Text = .Fields("NewMesto").Value
                    oTable.Cell(intj, 3).Range.Text = .Fields("Prich").Value
                    oTable.Cell(intj, 4).Range.Text = .Fields("data").Value
                    intj = intj + 1

                    .MoveNext()
                    'DoEvents
                Loop
            End With
            rscount.Close()
            rscount = Nothing


        End If

        '#####################################################################################
        'РЕМОНТЫ

        rscount = New ADODB.Recordset
        rscount.Open("SELECT COUNT(*) AS total_number FROM Remont WHERE id_comp=" & sID, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

        With rscount
            GIST = .Fields("total_number").Value
        End With

        rscount.Close()
        rscount = Nothing


        If GIST > 0 Then

            oPara1 = oDoc.Content.Paragraphs.Add
            oPara1.Range.Text = ""
            oPara1.Range.Font.Bold = True
            oPara1.Format.SpaceAfter = 12    '24 pt spacing after paragraph.
            oPara1.Range.InsertParagraphAfter()

            oPara1 = oDoc.Content.Paragraphs.Add
            oPara1.Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG44", "")
            oPara1.Range.Font.Bold = True
            oPara1.Format.SpaceAfter = 12    '24 pt spacing after paragraph.
            oPara1.Range.InsertParagraphAfter()

            oTable = oDoc.Tables.Add(oDoc.Bookmarks.Item("\endofdoc").Range, GIST + 1, 5)
            oTable.Range.ParagraphFormat.SpaceAfter = 6

            oTable.Cell(1, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG43", "")
            oTable.Cell(1, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG46", "")
            oTable.Cell(1, 3).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG47", "")
            oTable.Cell(1, 4).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG48", "")
            oTable.Cell(1, 5).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG49", "")

            rscount = New ADODB.Recordset
            rscount.Open("SELECT * FROM Remont WHERE id_comp=" & sID & " ORDER by Date", DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

            'Dim A As String

            intj = 2
            With rscount
                .MoveFirst()
                Do While Not .EOF = True

                    oTable.Cell(intj, 1).Range.Text = .Fields("Date").Value
                    oTable.Cell(intj, 2).Range.Text = .Fields("Remont").Value
                    oTable.Cell(intj, 3).Range.Text = .Fields("Master").Value
                    oTable.Cell(intj, 4).Range.Text = .Fields("Uroven").Value
                    oTable.Cell(intj, 5).Range.Text = .Fields("vip").Value
                    intj = intj + 1


                    .MoveNext()
                    'DoEvents
                Loop
            End With
            rscount.Close()
            rscount = Nothing



        End If


        oPara1 = oDoc.Content.Paragraphs.Add
        oPara1.Range.Text = ""
        oPara1.Range.Font.Bold = True
        oPara1.Format.SpaceAfter = 12    '24 pt spacing after paragraph.
        oPara1.Range.InsertParagraphAfter()

        oPara1 = oDoc.Content.Paragraphs.Add
        oPara1.Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG50", "")
        oPara1.Range.Font.Bold = True
        oPara1.Format.SpaceAfter = 12    '24 pt spacing after paragraph.
        oPara1.Range.InsertParagraphAfter()

        oTable = oDoc.Tables.Add(oDoc.Bookmarks.Item("\endofdoc").Range, 2, 2)
        oTable.Range.ParagraphFormat.SpaceAfter = 6

        oTable.Cell(1, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG51", "")
        oTable.Cell(1, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG52", "")

        CONFIGURE = New ADODB.Recordset
        CONFIGURE.Open("SELECT * FROM CONFIGURE", DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
        With CONFIGURE
            If .RecordCount <> 0 Then
                If Not IsDBNull(.Fields("SISADM")) Then uname = .Fields("SISADM").Value
            Else
                uname = "Nothing"
            End If
        End With
        CONFIGURE.Close()
        CONFIGURE = Nothing

        oTable.Cell(2, 1).Range.Text = uname
        oTable.Cell(2, 2).Range.Text = cOTV

    End Sub

    Public Sub blanks_my_wrd(ByVal tipot As String)
        Dim sSQL, sSQL1, SISADM, Organ As String
        Dim LNGIniFile As New IniFile(sLANGPATH)
        Dim uname As String



        If Len(tipot) = 0 Then Exit Sub
        On Error Resume Next

        Dim rs1 As ADODB.Recordset
        sSQL1 = "SELECT * FROM CONFIGURE"
        rs1 = New ADODB.Recordset
        rs1.Open(sSQL1, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

        With rs1
            Organ = .Fields("org").Value
            SISADM$ = .Fields("SISADM").Value
        End With
        rs1.Close()
        rs1 = Nothing


        Dim rs As ADODB.Recordset
        sSQL = "SELECT * FROM kompy WHERE id = " & frmComputers.sCOUNT
        rs = New ADODB.Recordset
        rs.Open(sSQL, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

        Dim intj As String
        Dim A, B, C, D, E As String

        Dim Wrd As Word.Application
        Dim WrdDc As Word.Document
        Wrd = New Word.Application

        Dim oDoc As Word.Document
        Dim oTable As Word.Table
        Dim oPara1 As Word.Paragraph, oPara2 As Word.Paragraph
        Dim oPara3 As Word.Paragraph, oPara4 As Word.Paragraph
        Dim oRng As Word.Range
        Dim oShape As Word.InlineShape
        Dim oChart As Object
        Dim Pos As Double

        WrdDc = Wrd.Documents.Open(tipot, , False)  'Application.
        WrdDc.Application.Visible = True

        Wrd.Selection.Find.ClearFormatting()
        Wrd.Selection.Find.Replacement.ClearFormatting()

        With rs
            .MoveFirst()
            Do While Not .EOF = True

                With Wrd.Selection.Find
                    .Text = "Организация"
                    .Replacement.Text = Organ
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "Сисадмин"
                    .Replacement.Text = SISADM
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("Ser_N_SIS").Value

                With Wrd.Selection.Find
                    .Text = "Ser_N_SIS"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("ID").Value

                With Wrd.Selection.Find
                    .Text = "ID"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)


                uname = .Fields("nomerPC").Value

                With Wrd.Selection.Find
                    .Text = "nomerPC"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CPU1").Value

                With Wrd.Selection.Find
                    .Text = "CPU1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CPUmhz1").Value

                With Wrd.Selection.Find
                    .Text = "CPUmhz1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CPUSocket1").Value

                With Wrd.Selection.Find
                    .Text = "CPUSocket1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CPUProizv1").Value

                With Wrd.Selection.Find
                    .Text = "CPUProizv1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CPU2").Value

                With Wrd.Selection.Find
                    .Text = "CPU2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CPUmhz2").Value

                With Wrd.Selection.Find
                    .Text = "CPUmhz2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CPUSocket2").Value

                With Wrd.Selection.Find
                    .Text = "CPUSocket2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CPUProizv2").Value

                With Wrd.Selection.Find
                    .Text = "CPUProizv2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CPU3").Value

                With Wrd.Selection.Find
                    .Text = "CPU3"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CPUmhz3").Value

                With Wrd.Selection.Find
                    .Text = "CPUmhz3"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CPUSocket3").Value

                With Wrd.Selection.Find
                    .Text = "CPUSocket3"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CPUProizv3").Value

                With Wrd.Selection.Find
                    .Text = "CPUProizv3"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CPU4").Value

                With Wrd.Selection.Find
                    .Text = "CPU4"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CPUmhz4").Value

                With Wrd.Selection.Find
                    .Text = "CPUmhz4"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CPUSocket4").Value

                With Wrd.Selection.Find
                    .Text = "CPUSocket4"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CPUProizv4").Value

                With Wrd.Selection.Find
                    .Text = "CPUProizv4"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("Mb").Value

                With Wrd.Selection.Find
                    .Text = "Mb1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("Mb_Chip").Value

                With Wrd.Selection.Find
                    .Text = "Mb_Chip"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("Mb_Id").Value

                With Wrd.Selection.Find
                    .Text = "Mb_Iz"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("Mb_Proizvod").Value

                With Wrd.Selection.Find
                    .Text = "Mb_Proizvod"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("RAM_1").Value
                With Wrd.Selection.Find
                    .Text = "RAM_1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("RAM_SN_1").Value
                With Wrd.Selection.Find
                    .Text = "RAM_SN_1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("RAM_speed_1").Value
                With Wrd.Selection.Find
                    .Text = "RAM_speed_1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("RAM_PROIZV_1").Value
                With Wrd.Selection.Find
                    .Text = "RAM_PROIZV_1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)


                uname = .Fields("RAM_2").Value
                With Wrd.Selection.Find
                    .Text = "RAM_2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("RAM_SN_2").Value
                With Wrd.Selection.Find
                    .Text = "RAM_SN_2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("RAM_speed_2").Value
                With Wrd.Selection.Find
                    .Text = "RAM_speed_2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("RAM_PROIZV_2").Value
                With Wrd.Selection.Find
                    .Text = "RAM_PROIZV_2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("RAM_3").Value
                With Wrd.Selection.Find
                    .Text = "RAM_3"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("RAM_SN_3").Value
                With Wrd.Selection.Find
                    .Text = "RAM_SN_3"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("RAM_speed_3").Value
                With Wrd.Selection.Find
                    .Text = "RAM_speed_3"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("RAM_PROIZV_3").Value
                With Wrd.Selection.Find
                    .Text = "RAM_PROIZV_3"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("RAM_4").Value
                With Wrd.Selection.Find
                    .Text = "RAM_4"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("RAM_SN_4").Value
                With Wrd.Selection.Find
                    .Text = "RAM_SN_4"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("RAM_speed_4").Value
                With Wrd.Selection.Find
                    .Text = "RAM_speed_4"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("RAM_PROIZV_4").Value
                With Wrd.Selection.Find
                    .Text = "RAM_PROIZV_4"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("HDD_Name_1").Value
                With Wrd.Selection.Find
                    .Text = "HDD_Name_1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("HDD_OB_1").Value
                With Wrd.Selection.Find
                    .Text = "HDD_OB_1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("HDD_SN_1").Value
                With Wrd.Selection.Find
                    .Text = "HDD_SN_1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("HDD_PROIZV_1").Value
                With Wrd.Selection.Find
                    .Text = "HDD_PROIZV_1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("HDD_Name_2").Value
                With Wrd.Selection.Find
                    .Text = "HDD_Name_2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("HDD_OB_2").Value
                With Wrd.Selection.Find
                    .Text = "HDD_OB_2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("HDD_SN_2").Value
                With Wrd.Selection.Find
                    .Text = "HDD_SN_2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("HDD_PROIZV_2").Value
                With Wrd.Selection.Find
                    .Text = "HDD_PROIZV_2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)


                uname = .Fields("HDD_Name_3").Value
                With Wrd.Selection.Find
                    .Text = "HDD_Name_3"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("HDD_OB_3").Value
                With Wrd.Selection.Find
                    .Text = "HDD_OB_3"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("HDD_SN_3").Value
                With Wrd.Selection.Find
                    .Text = "HDD_SN_3"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("HDD_PROIZV_3").Value
                With Wrd.Selection.Find
                    .Text = "HDD_PROIZV_3"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)


                uname = .Fields("HDD_Name_4").Value
                With Wrd.Selection.Find
                    .Text = "HDD_Name_4"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("HDD_OB_4").Value
                With Wrd.Selection.Find
                    .Text = "HDD_OB_4"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("HDD_SN_4").Value
                With Wrd.Selection.Find
                    .Text = "HDD_SN_4"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("HDD_PROIZV_4").Value
                With Wrd.Selection.Find
                    .Text = "HDD_PROIZV_4"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("SVGA_NAME").Value
                With Wrd.Selection.Find
                    .Text = "SVGA_NAME"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("SVGA_OB_RAM").Value
                With Wrd.Selection.Find
                    .Text = "SVGA_OB_RAM"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("SVGA_SN").Value
                With Wrd.Selection.Find
                    .Text = "SVGA_SN"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("SVGA_PROIZV").Value
                With Wrd.Selection.Find
                    .Text = "SVGA_PROIZV"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("SOUND_NAME").Value
                With Wrd.Selection.Find
                    .Text = "SOUND_NAME"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("SOUND_SN").Value
                With Wrd.Selection.Find
                    .Text = "SOUND_SN"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("SOUND_PROIZV").Value
                With Wrd.Selection.Find
                    .Text = "SOUND_PROIZV"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CD_NAME").Value
                With Wrd.Selection.Find
                    .Text = "CD_NAME"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CD_SPEED").Value
                With Wrd.Selection.Find
                    .Text = "CD_SPEED"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CD_SN").Value
                With Wrd.Selection.Find
                    .Text = "CD_SN"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CD_PROIZV").Value
                With Wrd.Selection.Find
                    .Text = "CD_PROIZV"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CDRW_NAME").Value
                With Wrd.Selection.Find
                    .Text = "CDRW_NAME"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CDRW_SPEED").Value
                With Wrd.Selection.Find
                    .Text = "CDRW_SPEED"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CDRW_SN").Value
                With Wrd.Selection.Find
                    .Text = "CDRW_SN"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("CDRW_PROIZV").Value
                With Wrd.Selection.Find
                    .Text = "CDRW_PROIZV"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("DVD_NAME").Value
                With Wrd.Selection.Find
                    .Text = "DVD_NAME"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("DVD_SPEED").Value
                With Wrd.Selection.Find
                    .Text = "DVD_SPEED"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("DVD_SN").Value
                With Wrd.Selection.Find
                    .Text = "DVD_SN"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("DVD_PROIZV").Value
                With Wrd.Selection.Find
                    .Text = "DVD_PROIZV"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("NET_NAME_1").Value
                With Wrd.Selection.Find
                    .Text = "NET_NAME_1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("NET_IP_1").Value
                With Wrd.Selection.Find
                    .Text = "NET_IP_1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("NET_MAC_1").Value
                With Wrd.Selection.Find
                    .Text = "NET_MAC_1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("NET_PROIZV_1").Value
                With Wrd.Selection.Find
                    .Text = "NET_PROIZV_1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)


                uname = .Fields("NET_NAME_2").Value
                With Wrd.Selection.Find
                    .Text = "NET_NAME_2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("NET_IP_2").Value
                With Wrd.Selection.Find
                    .Text = "NET_IP_2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("NET_MAC_2").Value
                With Wrd.Selection.Find
                    .Text = "NET_MAC_2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("NET_PROIZV_2").Value
                With Wrd.Selection.Find
                    .Text = "NET_PROIZV_2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("FDD_NAME").Value
                With Wrd.Selection.Find
                    .Text = "FDD_NAME"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("FDD_SN").Value
                With Wrd.Selection.Find
                    .Text = "FDD_SN"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("FDD_PROIZV").Value
                With Wrd.Selection.Find
                    .Text = "FDD_PROIZV"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("MODEM_NAME").Value
                With Wrd.Selection.Find
                    .Text = "MODEM_NAME"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("MODEM_SN").Value
                With Wrd.Selection.Find
                    .Text = "MODEM_SN"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("MODEM_PROIZV").Value
                With Wrd.Selection.Find
                    .Text = "MODEM_PROIZV"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("KEYBOARD_NAME").Value
                With Wrd.Selection.Find
                    .Text = "KEYBOARD_NAME"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("KEYBOARD_SN").Value
                With Wrd.Selection.Find
                    .Text = "KEYBOARD_SN"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("KEYBOARD_PROIZV").Value
                With Wrd.Selection.Find
                    .Text = "KEYBOARD_PROIZV"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("MOUSE_NAME").Value
                With Wrd.Selection.Find
                    .Text = "MOUSE_NAME"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("MOUSE_SN").Value
                With Wrd.Selection.Find
                    .Text = "MOUSE_SN"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("MOUSE_PROIZV").Value
                With Wrd.Selection.Find
                    .Text = "MOUSE_PROIZV"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("USB_NAME").Value
                With Wrd.Selection.Find
                    .Text = "USB_NAME"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("USB_SN").Value
                With Wrd.Selection.Find
                    .Text = "USB_SN"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("USB_PROIZV").Value
                With Wrd.Selection.Find
                    .Text = "USB_PROIZV"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("PCI_NAME").Value
                With Wrd.Selection.Find
                    .Text = "PCI_NAME"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("PCI_SN").Value
                With Wrd.Selection.Find
                    .Text = "PCI_SN"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("PCI_PROIZV").Value
                With Wrd.Selection.Find
                    .Text = "PCI_PROIZV"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("MONITOR_NAME").Value
                With Wrd.Selection.Find
                    .Text = "MONITOR_NAME"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("MONITOR_DUM").Value
                With Wrd.Selection.Find
                    .Text = "MONITOR_DUM"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("MONITOR_SN").Value
                With Wrd.Selection.Find
                    .Text = "MONITOR_SN"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("MONITOR_PROIZV").Value
                With Wrd.Selection.Find
                    .Text = "MONITOR_PROIZV"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("MONITOR_PROIZV").Value
                With Wrd.Selection.Find
                    .Text = "MONITOR_PROIZV"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("MONITOR_NAME2").Value
                With Wrd.Selection.Find
                    .Text = "MONITOR_NAME2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("MONITOR_DUM2").Value
                With Wrd.Selection.Find
                    .Text = "MONITOR_DUM2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("MONITOR_SN2").Value
                With Wrd.Selection.Find
                    .Text = "MONITOR_SN2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("MONITOR_PROIZV2").Value
                With Wrd.Selection.Find
                    .Text = "MONITOR_PROIZV2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("AS_NAME").Value
                With Wrd.Selection.Find
                    .Text = "AS_NAME"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("AS_SN").Value
                With Wrd.Selection.Find
                    .Text = "AS_SN"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("AS_PROIZV").Value
                With Wrd.Selection.Find
                    .Text = "AS_PROIZV"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("IBP_NAME").Value
                With Wrd.Selection.Find
                    .Text = "IBP_NAME"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("IBP_SN").Value
                With Wrd.Selection.Find
                    .Text = "IBP_SN"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("IBP_PROIZV").Value
                With Wrd.Selection.Find
                    .Text = "IBP_PROIZV"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("FILTR_NAME").Value
                With Wrd.Selection.Find
                    .Text = "FILTR_NAME"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("FILTR_SN").Value
                With Wrd.Selection.Find
                    .Text = "FILTR_SN"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("FILTR_PROIZV").Value
                With Wrd.Selection.Find
                    .Text = "FILTR_PROIZV"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("PRINTER_NAME_1").Value
                With Wrd.Selection.Find
                    .Text = "PRINTER_NAME_1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("PRINTER_SN_1").Value
                With Wrd.Selection.Find
                    .Text = "PRINTER_SN_1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("PORT_1").Value
                With Wrd.Selection.Find
                    .Text = "PORT_1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("PRINTER_PROIZV_1").Value
                With Wrd.Selection.Find
                    .Text = "PRINTER_PROIZV_1"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)


                uname = .Fields("PRINTER_NAME_2").Value
                With Wrd.Selection.Find
                    .Text = "PRINTER_NAME_2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("PRINTER_SN_2").Value
                With Wrd.Selection.Find
                    .Text = "PRINTER_SN_2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("PORT_2").Value
                With Wrd.Selection.Find
                    .Text = "PORT_2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("PRINTER_PROIZV_2").Value
                With Wrd.Selection.Find
                    .Text = "PRINTER_PROIZV_2"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("PRINTER_NAME_3").Value
                With Wrd.Selection.Find
                    .Text = "PRINTER_NAME_3"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("PRINTER_SN_3").Value
                With Wrd.Selection.Find
                    .Text = "PRINTER_SN_3"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("PORT_3").Value
                With Wrd.Selection.Find
                    .Text = "PORT_3"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("PRINTER_PROIZV_3").Value
                With Wrd.Selection.Find
                    .Text = "PRINTER_PROIZV_3"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("SCANER_NAME").Value
                With Wrd.Selection.Find
                    .Text = "SCANER_NAME"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("SCANER_SN").Value
                With Wrd.Selection.Find
                    .Text = "SCANER_SN"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("SCANER_PROIZV").Value
                With Wrd.Selection.Find
                    .Text = "SCANER_PROIZV"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("NET_NAME").Value
                With Wrd.Selection.Find
                    .Text = "NET_NAME"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("PSEVDONIM").Value
                With Wrd.Selection.Find
                    .Text = "PSEVDONIM"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("NET_NAME").Value
                With Wrd.Selection.Find
                    .Text = "#NETNAME"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("MESTO").Value
                With Wrd.Selection.Find
                    .Text = "Department"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("kabn").Value
                With Wrd.Selection.Find
                    .Text = "Office"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("TIP_COMPA").Value
                With Wrd.Selection.Find
                    .Text = "TIP_COMPA"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("FILIAL").Value
                With Wrd.Selection.Find
                    .Text = "Branch"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("TELEPHONE").Value
                With Wrd.Selection.Find
                    .Text = "TELEPHONE"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("INV_NO_SYSTEM").Value
                With Wrd.Selection.Find
                    .Text = "INV_NO_SYSTEM"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("INV_NO_PRINTER").Value
                With Wrd.Selection.Find
                    .Text = "INV_NO_PRINTER"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("INV_NO_MODEM").Value
                With Wrd.Selection.Find
                    .Text = "INV_NO_MODEM"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("INV_NO_SCANER").Value
                With Wrd.Selection.Find
                    .Text = "INV_NO_SCANER"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("INV_NO_MONITOR").Value
                With Wrd.Selection.Find
                    .Text = "INV_NO_MONITOR"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("INV_NO_IBP").Value
                With Wrd.Selection.Find
                    .Text = "INV_NO_IBP"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("OTvetstvennyj").Value
                With Wrd.Selection.Find
                    .Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG5", "")
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("BLOCK").Value
                With Wrd.Selection.Find
                    .Text = "BLOCK"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("SN_BLOCK").Value
                With Wrd.Selection.Find
                    .Text = "SN_BLOCK"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("PROIZV_BLOCK").Value
                With Wrd.Selection.Find
                    .Text = "PROIZV_BLOCK"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                uname = .Fields("SYS_PR").Value
                With Wrd.Selection.Find
                    .Text = "SYS_PR"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)


                uname = .Fields("StoimRub").Value
                With Wrd.Selection.Find
                    .Text = "StoimRub"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)


                uname = .Fields("DataVVoda").Value
                With Wrd.Selection.Find
                    .Text = "DataVVoda"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)



                Dim rs2 As ADODB.Recordset
                rs2 = New ADODB.Recordset
                rs2.Open("Select Postav FROM Garantia_sis WHERE Id_Comp=" & .Fields("id").Value, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

                With rs2
                    uname = .Fields("Postav").Value
                End With
                rs2.Close()
                rs2 = Nothing


                With Wrd.Selection.Find
                    .Text = "Postav"
                    .Replacement.Text = uname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                oPara1 = oDoc.Content.Paragraphs.Add
                oPara1.Range.Text = ""
                oPara1.Range.Font.Bold = True
                oPara1.Format.SpaceAfter = 24    '24 pt spacing after paragraph.
                oPara1.Range.InsertParagraphAfter()

                .MoveNext()
                'DoEvents
            Loop
        End With
        rs.Close()
        rs = Nothing

        'В составе устройства
        '#####################################################################################



        Dim GIST As String


        Dim objIniFile As New IniFile(PrPath & "base.ini")
        uname = objIniFile.GetString("MYBLANK", "VSU", "0")

        If uname = 1 Then



            sSQL1 = "SELECT count(*) as t_n FROM kompy WHERE PCL=" & frmComputers.sCOUNT

            rs1 = New ADODB.Recordset
            rs1.Open(sSQL1, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

            With rs1

                GIST = .Fields("t_n").Value

            End With

            rs1.Close()
            rs1 = Nothing

            If GIST = 0 Then

            Else


                oPara1 = WrdDc.Content.Paragraphs.Add
                oPara1.Range.Text = ""
                oPara1.Range.Font.Bold = True
                oPara1.Format.SpaceAfter = 24    '24 pt spacing after paragraph.
                oPara1.Range.InsertParagraphAfter()

                oPara1 = WrdDc.Content.Paragraphs.Add
                oPara1.Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG28", "")
                oPara1.Range.Font.Bold = True
                oPara1.Format.SpaceAfter = 24    '24 pt spacing after paragraph.
                oPara1.Range.InsertParagraphAfter()

                'iCOUNTFIELLDS + 1


                oTable = WrdDc.Tables.Add(WrdDc.Bookmarks.Item("\endofdoc").Range, GIST + 1, 4)
                oTable.Range.ParagraphFormat.SpaceAfter = 6

                oTable.Cell(1, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG29", "")
                oTable.Cell(1, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG10", "")
                oTable.Cell(1, 3).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG31", "")
                oTable.Cell(1, 4).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG8", "")

                sSQL1 = "SELECT * FROM kompy WHERE PCL=" & frmComputers.sCOUNT
                rs1 = New ADODB.Recordset
                rs1.Open(sSQL1, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

                intj = 2

                With rs1
                    .MoveFirst()
                    Do While Not .EOF

                        Select Case .Fields("tiptehn").Value

                            Case "Printer"
                                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG27", "")
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_SN_1").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("INV_NO_PRINTER").Value

                            Case "MFU"
                                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG32", "")
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_SN_1").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("INV_NO_PRINTER").Value

                            Case "MONITOR"
                                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG24", "")
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("MONITOR_SN").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("INV_NO_MONITOR").Value

                            Case "OT"
                                oTable.Cell(intj, 1).Range.Text = .Fields("TIP_COMPA").Value
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_SN_1").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("INV_NO_PRINTER").Value

                            Case "SCANER"
                                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG34", "")
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_SN_1").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("INV_NO_PRINTER").Value


                            Case "USB"
                                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG37", "")
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_SN_1").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("INV_NO_PRINTER").Value

                            Case "IBP"
                                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG35", "")
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_SN_1").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("INV_NO_PRINTER").Value

                            Case "FS"
                                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG26", "")
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_SN_1").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("INV_NO_PRINTER").Value

                            Case "SOUND"
                                oTable.Cell(intj, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG25", "")
                                oTable.Cell(intj, 2).Range.Text = .Fields("NET_NAME").Value
                                oTable.Cell(intj, 3).Range.Text = .Fields("PRINTER_SN_1").Value
                                oTable.Cell(intj, 4).Range.Text = .Fields("INV_NO_PRINTER").Value

                        End Select

                        intj = intj + 1

                        .MoveNext()
                    Loop

                End With
                rs1.Close()
                rs1 = Nothing

            End If

        End If


        'Установленное ПО

        uname = objIniFile.GetString("MYBLANK", "POu", "0")

        If uname = 1 Then

            sSQL1 = "SELECT count(*) as t_n FROM SOFT_INSTALL WHERE id_comp=" & frmComputers.sCOUNT

            rs1 = New ADODB.Recordset
            rs1.Open(sSQL1, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

            With rs1

                GIST = .Fields("t_n").Value

            End With

            rs1.Close()
            rs1 = Nothing

            If GIST = 0 Then
            Else

                oPara1 = WrdDc.Content.Paragraphs.Add
                oPara1.Range.Text = ""
                oPara1.Range.Font.Bold = True
                oPara1.Format.SpaceAfter = 24    '24 pt spacing after paragraph.
                oPara1.Range.InsertParagraphAfter()

                oPara1 = WrdDc.Content.Paragraphs.Add
                oPara1.Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG38", "")
                oPara1.Range.Font.Bold = True
                oPara1.Format.SpaceAfter = 24    '24 pt spacing after paragraph.
                oPara1.Range.InsertParagraphAfter()


                'iCOUNTFIELLDS + 1

                oTable = WrdDc.Tables.Add(WrdDc.Bookmarks.Item("\endofdoc").Range, GIST + 1, 1)
                oTable.Range.ParagraphFormat.SpaceAfter = 6

                oTable.Cell(1, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG38", "")

                intj = 2

                sSQL1 = "SELECT * FROM SOFT_INSTALL WHERE id_comp=" & frmComputers.sCOUNT
                rs1 = New ADODB.Recordset
                rs1.Open(sSQL1, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
                With rs1
                    If .RecordCount <> 0 Then
                        .MoveFirst()
                        Do While Not .EOF

                            oTable.Cell(intj, 1).Range.Text = .Fields("Soft").Value
                            intj = intj + 1

                            .MoveNext()
                        Loop
                    End If
                End With

                rs1.Close()
                rs1 = Nothing


            End If

        End If





        '#####################################################################################
        'ПЕРЕМЕЩЕНИЕ ТЕХНИКИ
        Dim rscount As ADODB.Recordset 'Объявляем рекордсет

        uname = objIniFile.GetString("MYBLANK", "DVIG", "0")

        If uname = 1 Then

            rscount = New ADODB.Recordset
            rscount.Open("SELECT COUNT(*) AS total_number FROM dvig WHERE id_comp=" & frmComputers.sCOUNT, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

            With rscount
                GIST = .Fields("total_number").Value
            End With

            rscount.Close()
            rscount = Nothing


            If GIST > 0 Then

                oPara1 = WrdDc.Content.Paragraphs.Add
                oPara1.Range.Text = ""
                oPara1.Range.Font.Bold = True
                oPara1.Format.SpaceAfter = 24    '24 pt spacing after paragraph.
                oPara1.Range.InsertParagraphAfter()

                oPara1 = WrdDc.Content.Paragraphs.Add
                oPara1.Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG39", "")
                oPara1.Range.Font.Bold = True
                oPara1.Format.SpaceAfter = 12    '24 pt spacing after paragraph.
                oPara1.Range.InsertParagraphAfter()

                oTable = WrdDc.Tables.Add(WrdDc.Bookmarks.Item("\endofdoc").Range, GIST + 1, 4)
                oTable.Range.ParagraphFormat.SpaceAfter = 6

                oTable.Cell(1, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG40", "")
                oTable.Cell(1, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG41", "")
                oTable.Cell(1, 3).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG42", "")
                oTable.Cell(1, 4).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG43", "")

                intj = 2
                rscount = New ADODB.Recordset
                rscount.Open("SELECT * FROM dvig WHERE id_comp=" & frmComputers.sCOUNT, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

                intj = 2
                With rscount
                    .MoveFirst()
                    Do While Not .EOF = True

                        oTable.Cell(intj, 1).Range.Text = .Fields("oldMesto").Value
                        oTable.Cell(intj, 2).Range.Text = .Fields("NewMesto").Value
                        oTable.Cell(intj, 3).Range.Text = .Fields("Prich").Value
                        oTable.Cell(intj, 4).Range.Text = .Fields("data").Value
                        intj = intj + 1

                        .MoveNext()
                        'DoEvents
                    Loop
                End With
                rscount.Close()
                rscount = Nothing


            End If

        End If

        '#####################################################################################
        'РЕМОНТЫ

        uname = objIniFile.GetString("MYBLANK", "REMONT", "0")

        If uname = 1 Then

            rscount = New ADODB.Recordset
            rscount.Open("SELECT COUNT(*) AS total_number FROM Remont WHERE id_comp=" & frmComputers.sCOUNT, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

            With rscount
                GIST = .Fields("total_number").Value
            End With

            rscount.Close()
            rscount = Nothing


            If GIST > 0 Then

                oPara1 = WrdDc.Content.Paragraphs.Add
                oPara1.Range.Text = ""
                oPara1.Range.Font.Bold = True
                oPara1.Format.SpaceAfter = 12    '24 pt spacing after paragraph.
                oPara1.Range.InsertParagraphAfter()

                oPara1 = WrdDc.Content.Paragraphs.Add
                oPara1.Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG44", "")
                oPara1.Range.Font.Bold = True
                oPara1.Format.SpaceAfter = 12    '24 pt spacing after paragraph.
                oPara1.Range.InsertParagraphAfter()

                oTable = WrdDc.Tables.Add(WrdDc.Bookmarks.Item("\endofdoc").Range, GIST + 1, 5)
                oTable.Range.ParagraphFormat.SpaceAfter = 6

                oTable.Cell(1, 1).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG43", "")
                oTable.Cell(1, 2).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG46", "")
                oTable.Cell(1, 3).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG47", "")
                oTable.Cell(1, 4).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG48", "")
                oTable.Cell(1, 5).Range.Text = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG49", "")

                rscount = New ADODB.Recordset
                rscount.Open("SELECT * FROM Remont WHERE id_comp=" & frmComputers.sCOUNT, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

                'Dim A As String

                intj = 2
                With rscount
                    .MoveFirst()
                    Do While Not .EOF = True

                        oTable.Cell(intj, 1).Range.Text = .Fields("Date").Value
                        oTable.Cell(intj, 2).Range.Text = .Fields("Remont").Value
                        oTable.Cell(intj, 3).Range.Text = .Fields("Master").Value
                        oTable.Cell(intj, 4).Range.Text = .Fields("Uroven").Value
                        oTable.Cell(intj, 5).Range.Text = .Fields("vip").Value
                        intj = intj + 1


                        .MoveNext()
                        'DoEvents
                    Loop
                End With
                rscount.Close()
                rscount = Nothing



            End If
        End If


    End Sub

    Public Function ExportListViewToExcel(ByVal MyListView As ListView)

        ' Dim ExcelReport As Excel.ApplicationClass

        Const MAX_COLOURS As Int16 = 40

        Const MAX_COLUMS As Int16 = 254

        Dim i As Integer
        Dim New_Item As Windows.Forms.ListViewItem

        Dim TempColum As Int16
        Dim ColumLetter As String
        Dim TempRow As Int16
        Dim TempColum2 As Int16

        Dim AddedColours As Int16 = 1
        Dim MyColours As Hashtable = New Hashtable

        Dim AddNewBackColour As Boolean = True
        Dim AddNewFrontColour As Boolean = True
        Dim BackColour As String
        Dim FrontColour As String

        Dim ExcelReport As Excel.Application

        'ExcelReport = New Excel.ApplicationClass
        ExcelReport = New Excel.Application


        ExcelReport.Visible = True

        ExcelReport.Workbooks.Add()
        'ExcelReport.Worksheets("Sheet1").Select()
        ' ExcelReport.Sheets("Sheet1").Name = MyListView.Name

        i = 0
        Do Until i = MyListView.Columns.Count

            If i > MAX_COLUMS Then

                MsgBox("Too many Colums added")
                Exit Do

            End If
            TempColum = i
            TempColum2 = 0
            Do While TempColum > 25

                TempColum -= 26
                TempColum2 += 1

            Loop

            ColumLetter = Chr(97 + TempColum)
            If TempColum2 > 0 Then ColumLetter = Chr(96 + TempColum2) & ColumLetter


            ExcelReport.Range(ColumLetter & 1).Value = MyListView.Columns(i).Text
            ExcelReport.Range(ColumLetter & 1).Font.Name = MyListView.Font.Name
            ExcelReport.Range(ColumLetter & 1).Font.Size = MyListView.Font.Size
            i += 1

        Loop

        TempRow = 2
        For Each New_Item In MyListView.Items

            i = 0
            Do Until i = New_Item.SubItems.Count

                If i > MAX_COLUMS Then

                    MsgBox("Too many Colums added")
                    Exit Do

                End If

                TempColum = i
                TempColum2 = 0

                Do While TempColum > 25

                    TempColum -= 26
                    TempColum2 += 1

                Loop

                ColumLetter = Chr(97 + TempColum)
                If TempColum2 > 0 Then ColumLetter = Chr(96 + TempColum2) & ColumLetter

                ExcelReport.Range(ColumLetter & TempRow).Value = New_Item.SubItems(i).Text
                ExcelReport.Range(ColumLetter & TempRow).Font.Name = New_Item.Font.Name
                ExcelReport.Range(ColumLetter & TempRow).Font.Size = New_Item.Font.Size

                AddNewFrontColour = False
                AddNewBackColour = False
                Try

                    BackColour = MyColours(New_Item.BackColor.ToString)
                    If BackColour = "" Then AddNewBackColour = True

                    FrontColour = MyColours(New_Item.ForeColor.ToString)
                    If FrontColour = "" Then AddNewFrontColour = True

                Catch ex As Exception
                    AddNewFrontColour = False
                    AddNewBackColour = False

                End Try

                If AddedColours < MAX_COLOURS And (AddNewFrontColour Or AddNewBackColour) And (New_Item.BackColor.ToArgb <> -1) Then

                    If AddNewBackColour Then

                        MyColours.Add(New_Item.BackColor.ToString, AddedColours)
                        ExcelReport.Workbooks.Item(1).Colors(AddedColours) = RGB(New_Item.BackColor.R, New_Item.BackColor.G, New_Item.BackColor.B)
                        AddedColours += 1

                    End If


                    If AddNewFrontColour Then

                        MyColours.Add(New_Item.ForeColor.ToString, AddedColours)
                        ExcelReport.Workbooks.Item(1).Colors(AddedColours) = RGB(New_Item.ForeColor.R, New_Item.ForeColor.G, New_Item.ForeColor.B)
                        AddedColours += 1

                    End If
                End If

                ExcelReport.Rows(TempRow & ":" & TempRow).select()
                ExcelReport.Selection.Interior.ColorIndex = MyColours(New_Item.BackColor.ToString)
                ExcelReport.Selection.Font.ColorIndex = MyColours(New_Item.ForeColor.ToString)

                i += 1

            Loop
            TempRow += 1

        Next

        ExcelReport.Cells.Select()
        ExcelReport.Cells.EntireColumn.AutoFit()
        ExcelReport.Cells.Range("A1").Select()


    End Function

    Public Sub ExportListViewToCalc(ByVal listV As ListView)
        Dim replacer As Object
        Dim objServiceManager, objDesktop, Cell As Object 'ODS, Sheets,
        Dim objCoreReflection, oDoc, oSheet As Object 'objDocument,
        'Dim objText, objCursor As Object
        ' Dim objTable As Object
        Dim oCol As Object 'objRows, objRow,

        objServiceManager = CreateObject("com.sun.star.ServiceManager")
        objCoreReflection = objServiceManager.createInstance("com.sun.star.reflection.CoreReflection")


        objDesktop = objServiceManager.createInstance("com.sun.star.frame.Desktop")


        Dim args(-1) As Object

        '%$$$$$$$$$$$$
        objServiceManager = CreateObject("com.sun.star.ServiceManager")
        objDesktop = objServiceManager.createInstance("com.sun.star.frame.Desktop")

        Dim CurrentHeader As ColumnHeader

        oDoc = objDesktop.loadComponentFROMURL("private:factory/scalc", "_blank", 0, args)

        oSheet = oDoc.getSheets().getByIndex(0)


        Dim intj As Integer = 0
        Dim a, b As String

        intj = 1

        For Each CurrentHeader In listV.Columns

            a = "A" & intj
            b = CurrentHeader.Text
            oSheet.getCellByPosition(intj - 1, 0).setString(b)


            intj = intj + 1
        Next

        Dim STrX As Integer
        STrX = intj - 2

        listV.Select()
        For intj = 0 To listV.Items.Count - 1

            listV.Items(intj).Selected = True
            listV.Items(intj).EnsureVisible()

            For INTZ = 1 To STrX
                Cell = oSheet.getCellByPosition(0, intj + 1).setString(listV.Items.Item(intj).Text)

                Cell = oSheet.getCellByPosition(INTZ, intj + 1).setString(listV.Items(intj).SubItems(INTZ).Text)

                oCol = oSheet.getColumns().getByIndex(INTZ - 1)
                oCol.optimalWidth = True 'Autofit
                oCol = oSheet.getColumns().getByIndex(STrX)
                oCol.optimalWidth = True 'Autofit
            Next



        Next

        'replacer = oSheet.getByIndex().createReplaceDescriptor
        'replacer.setSearchString(".*")
        'replacer.setReplaceString("&")
        'Debug.Print(oDoc.replaceAll(replacer))


    End Sub

    Public Sub SRASP(ByVal sSID As String)
        Dim LNGIniFile As New IniFile(sLANGPATH)


        Dim tipot, sSQL As String

        tipot = Directory.GetParent(Application.ExecutablePath).ToString & "\blanks\SRASP.doc"

        Dim rs As ADODB.Recordset
        sSQL = "SELECT * FROM Remont WHERE id = " & sSID

        rs = New ADODB.Recordset
        rs.Open(sSQL, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)


        Dim sTEXT, sMASTER, sISTOCHNIK, sDATE, sMODEL, sSERNUMBER, sTIP, sTIP2, sOBJECT As String
        Dim sIDCMP As Integer

        With rs
            sTEXT = .Fields("Remont").Value
            sMASTER = .Fields("Master").Value

            sISTOCHNIK = .Fields("istochnik").Value
            sDATE = .Fields("Date").Value
            sIDCMP = .Fields("Id_Comp").Value

        End With
        rs.Close()
        rs = Nothing

        sSQL = "SELECT * FROM kompy WHERE id = " & sIDCMP
        rs = New ADODB.Recordset
        rs.Open(sSQL, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

        With rs
            sTIP = .Fields("TipTehn").Value

            Select Case sTIP

                Case "PC"

                    sSERNUMBER = .Fields("Ser_N_SIS").Value
                    sOBJECT = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG66", "")
                Case "Printer"

                    sSERNUMBER = .Fields("PRINTER_SN_1").Value
                    sOBJECT = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG27", "") & " " & .Fields("NET_NAME").Value
                Case "KOpir"

                    sSERNUMBER = .Fields("PRINTER_SN_1").Value
                    sOBJECT = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG60", "") & " " & .Fields("NET_NAME").Value
                Case "MONITOR"
                    sSERNUMBER = .Fields("MONITOR_SN").Value
                    sOBJECT = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG24", "") & " " & .Fields("NET_NAME").Value
                Case "SCANER"
                    sSERNUMBER = .Fields("PRINTER_SN_1").Value
                    sOBJECT = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG34", "") & " " & .Fields("NET_NAME").Value
                Case "NET"
                    sSERNUMBER = .Fields("port_1").Value
                    sOBJECT = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG61", "") & " " & .Fields("NET_NAME").Value
                Case "PHOTO"
                    sSERNUMBER = .Fields("PRINTER_SN_1").Value
                    sOBJECT = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG62", "") & " " & .Fields("NET_NAME").Value
                Case "OT"
                    sSERNUMBER = .Fields("PRINTER_SN_1").Value
                    sMODEL = .Fields("NET_NAME").Value
                    sTIP2 = .Fields("TIP_COMPA").Value
                    sOBJECT = .Fields("NET_NAME").Value
                    sOBJECT = sOBJECT & " " & sTIP2
                Case "ZIP"
                    sSERNUMBER = .Fields("PRINTER_SN_1").Value
                    sOBJECT = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG65", "") & " " & .Fields("NET_NAME").Value
                Case "PHONE"
                    sSERNUMBER = .Fields("PRINTER_SN_1").Value
                    sOBJECT = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG63", "") & " " & .Fields("NET_NAME").Value
                Case "MFU"
                    sSERNUMBER = .Fields("PRINTER_SN_1").Value
                    sOBJECT = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG32", "") & " " & .Fields("NET_NAME").Value
                Case "FAX"
                    sSERNUMBER = .Fields("PRINTER_SN_1").Value
                    sOBJECT = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG64", "") & " " & .Fields("NET_NAME").Value

                Case "USB"

                    sSERNUMBER = .Fields("PRINTER_SN_1").Value
                    sOBJECT = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG37", "") & " " & .Fields("NET_NAME").Value
                Case "IBP"

                    sSERNUMBER = .Fields("PRINTER_SN_1").Value
                    sOBJECT = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG35", "") & " " & .Fields("NET_NAME").Value
                Case "FS"
                    sSERNUMBER = .Fields("PRINTER_SN_1").Value
                    sOBJECT = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG262", "") & " " & .Fields("NET_NAME").Value

                Case "SOUND"
                    sSERNUMBER = .Fields("PRINTER_SN_1").Value
                    sOBJECT = LNGIniFile.GetString("MOD_OPENOFFICE", "MSG25", "") & " " & .Fields("NET_NAME").Value


            End Select


        End With
        rs.Close()
        rs = Nothing




        Select Case sOfficePACK

            Case "OpenOffice.org"
                Dim oSM As Object            'Root object for accessing OpenOffice FROM VB
                Dim oDesk, oDoc As Object 'First objects FROM the API
                Dim arg(-1) As Object             'Ignore it for the moment !

                oSM = CreateObject("com.sun.star.ServiceManager")


                'Create the first and most important service
                oDesk = oSM.createInstance("com.sun.star.frame.Desktop")
                'Create a new doc
                oDoc = oDesk.loadComponentFROMURL("file:///" & tipot, "_blank", 0, arg)
                ' jon code
                Dim objText As Object, objCursor As Object
                objText = oDoc.GetText
                objCursor = objText.createTextCursor

                ' replace all
                Dim oSrch As Object

                'Set oSrch = oDoc.createSearchDescriptor
                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("источник")
                oSrch.setReplaceString(sISTOCHNIK)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("Объект")
                oSrch.setReplaceString(sOBJECT)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("Модель")
                oSrch.setReplaceString(sMODEL)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("Дефект")
                oSrch.setReplaceString(sTEXT)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("Мастер")
                oSrch.setReplaceString(sMASTER)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("Число")
                oSrch.setReplaceString(sDATE)
                Debug.Print(oDoc.replaceAll(oSrch))

            Case Else

                Dim Wrd As Word.Application
                Dim WrdDc As Object
                Wrd = New Word.Application

                WrdDc = Wrd.Documents.Open(tipot, , False)  'Application.
                WrdDc.Application.Visible = True

                Wrd.Selection.Find.ClearFormatting()
                Wrd.Selection.Find.Replacement.ClearFormatting()
                'Номер



                With Wrd.Selection.Find
                    .Text = "источник"
                    .Replacement.Text = sISTOCHNIK
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "Объект"
                    .Replacement.Text = sOBJECT
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "Модель"
                    .Replacement.Text = sMODEL
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "Дефект"
                    .Replacement.Text = sTEXT
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "Мастер"
                    .Replacement.Text = sMASTER
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "Число"
                    .Replacement.Text = sDATE
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)
                WrdDc = Nothing
                Wrd = Nothing

        End Select
    End Sub

    Public Sub SRASP2(ByVal sSID As String)
        Dim LNGIniFile As New IniFile(sLANGPATH)

        If sSID = 0 Then Exit Sub

        Dim rs As ADODB.Recordset
        Dim tipot, sSQL As String

        tipot = Directory.GetParent(Application.ExecutablePath).ToString & "\blanks\akt_z.dot"


        Dim sTEXT, sMASTER, sISTOCHNIK, sDATE, sTIP, Sorganization, sMEMO, stTIME, stDATE, spTIME, spDATE, sRAB As String
        Dim sIDCMP As Integer


        sSQL = "SELECT * FROM Remont WHERE id = " & sSID
        rs = New ADODB.Recordset
        rs.Open(sSQL, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
        With rs
            If Not IsDBNull(.Fields("Remont").Value) Then sTEXT = .Fields("Remont").Value
            If Not IsDBNull(.Fields("Master").Value) Then sMASTER = .Fields("Master").Value
            If Not IsDBNull(.Fields("istochnik").Value) Then sISTOCHNIK = .Fields("istochnik").Value
            If Not IsDBNull(.Fields("Date").Value) Then sDATE = .Fields("Date").Value
            If Not IsDBNull(.Fields("Id_Comp").Value) Then sIDCMP = .Fields("Id_Comp").Value
            If Not IsDBNull(.Fields("Uroven").Value) Then sTIP = .Fields("Uroven").Value
            If Not IsDBNull(.Fields("MeMo").Value) Then sMEMO = .Fields("MeMo").Value

            If Not IsDBNull(.Fields("starttime").Value) Then stTIME = .Fields("starttime").Value
            If Not IsDBNull(.Fields("startdate").Value) Then stDATE = .Fields("startdate").Value

            If Not IsDBNull(.Fields("stoptime").Value) Then spTIME = .Fields("stoptime").Value
            If Not IsDBNull(.Fields("stopdate").Value) Then spDATE = .Fields("stopdate").Value

        End With
        rs.Close()
        rs = Nothing


        sSQL = "SELECT count(*) as t_n FROM remonty_plus WHERE id_rem = " & sSID
        Dim rCOUNTer As Integer
        rs = New ADODB.Recordset
        rs.Open(sSQL, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
        With rs
            .MoveFirst()
            Do While Not .EOF

                rCOUNTer = .Fields("t_n").Value

                .MoveNext()
            Loop
        End With
        rs.Close()
        rs = Nothing


        If rCOUNTer > 0 Then

            sSQL = "SELECT * FROM remonty_plus WHERE id_rem = " & sSID
            rs = New ADODB.Recordset
            rs.Open(sSQL, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
            With rs
                .MoveFirst()
                Do While Not .EOF

                    If Len(sRAB) = 0 Then
                        If Not IsDBNull(.Fields("otzyv").Value) Then sRAB = .Fields("otzyv").Value
                    Else
                        If Not IsDBNull(.Fields("otzyv").Value) Then sRAB = sRAB & ", " & .Fields("otzyv").Value
                    End If

                    .MoveNext()
                Loop
            End With
            rs.Close()
            rs = Nothing

        End If



        rs = New ADODB.Recordset
        rs.Open("select * from kompy where id=" & sIDCMP, DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

        Dim sINN, sBR, sDEP, sKab, Snname As String
        With rs
            If Not IsDBNull(.Fields("INV_NO_SYSTEM").Value) Then sINN = .Fields("INV_NO_SYSTEM").Value
            If Not IsDBNull(.Fields("FILIAL").Value) Then sBR = .Fields("FILIAL").Value
            If Not IsDBNull(.Fields("MESTO").Value) Then sDEP = .Fields("MESTO").Value
            If Not IsDBNull(.Fields("kabn").Value) Then sKab = .Fields("kabn").Value
            If Not IsDBNull(.Fields("NET_NAME").Value) Then Snname = .Fields("NET_NAME").Value

        End With
        rs.Close()
        rs = Nothing

        Dim rs1 As ADODB.Recordset
        rs1 = New ADODB.Recordset
        rs1.Open("SELECT * FROM CONFIGURE", DB7, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

        With rs1
            If Not IsDBNull(.Fields("org").Value) Then Sorganization = .Fields("org").Value
        End With
        rs1.Close()
        rs1 = Nothing




        Select Case sOfficePACK

            Case "OpenOffice.org"
                Dim oSM As Object            'Root object for accessing OpenOffice FROM VB
                Dim oDesk, oDoc As Object 'First objects FROM the API
                Dim arg(-1) As Object             'Ignore it for the moment !

                oSM = CreateObject("com.sun.star.ServiceManager")


                'Create the first and most important service
                oDesk = oSM.createInstance("com.sun.star.frame.Desktop")
                'Create a new doc
                oDoc = oDesk.loadComponentFROMURL("file:///" & tipot, "_blank", 0, arg)
                ' jon code
                Dim objText As Object, objCursor As Object
                objText = oDoc.GetText
                objCursor = objText.createTextCursor

                ' replace all
                Dim oSrch As Object

                'Sorganization

                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("#organization")
                oSrch.setReplaceString(Sorganization)
                Debug.Print(oDoc.replaceAll(oSrch))


                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("#number")
                oSrch.setReplaceString(sSID)
                Debug.Print(oDoc.replaceAll(oSrch))

                'Set oSrch = oDoc.createSearchDescriptor
                oSrch = oDoc.createReplaceDescriptor
                oSrch.setSearchString("#branche")
                oSrch.setReplaceString(sBR & "/" & sDEP & "/" & sKab)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("#tehn_name")
                oSrch.setReplaceString(Snname)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("#invnumber")
                oSrch.setReplaceString(sINN)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("#textzaiavki")
                oSrch.setReplaceString(sTEXT)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("#tip")
                oSrch.setReplaceString(sTIP)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("#istochnik")
                oSrch.setReplaceString(sISTOCHNIK)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("#dateisp")
                oSrch.setReplaceString(sDATE)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("#koment")
                oSrch.setReplaceString(sMEMO)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("#raboty")
                oSrch.setReplaceString(sRAB)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("#master")
                oSrch.setReplaceString(sMASTER)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("#starttime")
                oSrch.setReplaceString(stTIME)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("#startdate")
                oSrch.setReplaceString(stDATE)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("#stoptime")
                oSrch.setReplaceString(spTIME)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("#stopdate")
                oSrch.setReplaceString(spDATE)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("#date")
                oSrch.setReplaceString(Date.Today)
                Debug.Print(oDoc.replaceAll(oSrch))

                oSrch.setSearchString("#time")
                oSrch.setReplaceString(DateTime.Now.Hour & ":" & DateTime.Now.Minute)
                Debug.Print(oDoc.replaceAll(oSrch))

                '#date #time
                '#organization

            Case Else

                Dim Wrd As Word.Application
                Dim WrdDc As Object
                Wrd = New Word.Application

                WrdDc = Wrd.Documents.Open(tipot, , False)  'Application.
                WrdDc.Application.Visible = True

                Wrd.Selection.Find.ClearFormatting()
                Wrd.Selection.Find.Replacement.ClearFormatting()
                'Номер

                '#number
                With Wrd.Selection.Find
                    .Text = "#number"
                    .Replacement.Text = sSID
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)


                With Wrd.Selection.Find
                    .Text = "#organization"
                    .Replacement.Text = Sorganization
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "#branche"
                    .Replacement.Text = sBR & "/" & sDEP & "/" & sKab
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "#tehn_name"
                    .Replacement.Text = Snname
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "#invnumber"
                    .Replacement.Text = sINN
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "#textzaiavki"
                    .Replacement.Text = sTEXT
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "#tip"
                    .Replacement.Text = sTIP
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "#istochnik"
                    .Replacement.Text = sISTOCHNIK
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "#dateisp"
                    .Replacement.Text = sDATE
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)


                With Wrd.Selection.Find
                    .Text = "#koment"
                    .Replacement.Text = sMEMO
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "#raboty"
                    .Replacement.Text = sRAB
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "#master"
                    .Replacement.Text = sMASTER
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "#starttime"
                    .Replacement.Text = stTIME
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "#stopdate"
                    .Replacement.Text = spDATE
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "#stoptime"
                    .Replacement.Text = spTIME
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "#startdate"
                    .Replacement.Text = stDATE
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "#date"
                    .Replacement.Text = Date.Today
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                With Wrd.Selection.Find
                    .Text = "#time"
                    .Replacement.Text = DateTime.Now.Hour & ":" & DateTime.Now.Minute
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = True
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    ' .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                Wrd.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                '#date
                '#time

                WrdDc = Nothing
                Wrd = Nothing

        End Select
    End Sub

End Module

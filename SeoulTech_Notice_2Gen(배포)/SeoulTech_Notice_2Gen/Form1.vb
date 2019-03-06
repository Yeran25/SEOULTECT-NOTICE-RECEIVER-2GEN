Imports System.ComponentModel

Public Class Form1

    '개발자 정보
    '서울과학기술대학교 컴퓨터공학과 ITS
    'Blog : http://its319.tistory.com/
    'E-mail : ehn1225@seoultech.ac.kr
    '제작년일 : 2019.03.06

    Dim Winhttp As New WinHttp.WinHttpRequest
    Dim listview_page(7) As Integer
    Private listview(7) As ListView
    Private Combobox(2) As ComboBox

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Form2.Close()
    End Sub
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Process.Start("http://www.seoultech.ac.kr/index.jsp") '학교홈페이지 방문
    End Sub
    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Process.Start("http://its319.tistory.com/") 'ITS블로그 방문
    End Sub
    Public Sub New() '각종 변수 초기화 및 매칭
        InitializeComponent()
        listview(0) = ListView1
        listview(1) = ListView2
        listview(2) = ListView3
        listview(3) = ListView4
        listview(4) = ListView5
        listview(5) = ListView6
        listview(6) = ListView7
        Combobox(1) = ComboBox1
        Combobox(2) = ComboBox2
        For i = 0 To 6
            listview_page(i) = 1
        Next
    End Sub
    Private Sub Check_Tab_Name() '탭의 이름을 다시 불러옵니다.

        If My.Settings.Custom1_Url = "" Then
            ChromeTabcontrol1.TabPages.Item(4).Text = "Custom1"
            ComboBox1.Text = "없음"
        Else
            ChromeTabcontrol1.TabPages.Item(4).Text = LSet(My.Settings.Custom1_Name, 6)
            ComboBox1.Text = My.Settings.Custom1_Name
        End If
        If My.Settings.Custom2_Url = "" Then
            ChromeTabcontrol1.TabPages.Item(5).Text = "Custom2"
            ComboBox2.Text = "없음"
        Else
            ChromeTabcontrol1.TabPages.Item(5).Text = LSet(My.Settings.Custom2_Name, 6)
            ComboBox2.Text = My.Settings.Custom2_Name
        End If
        If My.Settings.Custom3_Url = "" Then
            ChromeTabcontrol1.TabPages.Item(6).Text = "Custom3"
            ComboBox3.Text = "없음"
        Else
            ChromeTabcontrol1.TabPages.Item(6).Text = LSet(My.Settings.Custom3_Name, 6)
            ComboBox3.Text = My.Settings.Custom3_Name
        End If

    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load '폼이 실행될때, 기본 셋팅값들을 로딩합니다.

        Dim T1 As DateTime = DateTime.Parse(“2019-03-16“) '라벨이 사라질 날짜
        Dim T2 As DateTime = DateTime.Parse(Now) '현재 시스템 날자
        If T1 < T2 Then '날짜가 되면 사라지는 알림용 
            Label6.Visible = False '라벨이 사라짐
        Else
            Label6.Visible = True '라벨이 보임
        End If

        Check_Tab_Name() '탭이름 재로딩

        Dim Url As String = ""
        For Tab_index = 0 To 6 '탭 전체 로딩(리스트뷰에 내용을 불러옵니다.)

            Select Case Tab_index
                Case 0
                    Url = "http://www.seoultech.ac.kr/service/info/notice/"
                Case 1
                    Url = "http://www.seoultech.ac.kr/service/info/matters/"
                Case 2
                    Url = "http://www.seoultech.ac.kr/service/info/janghak/"
                Case 3
                    Url = "http://www.seoultech.ac.kr/service/info/graduate/"
                Case 4
                    Url = My.Settings.Custom1_Url
                Case 5
                    Url = My.Settings.Custom2_Url
                Case 6
                    Url = My.Settings.Custom3_Url
            End Select

            If Url = "" Then 'URL이 등록되지 않은 경우
                listview(Tab_index).Items.Add("info")
                listview(Tab_index).Items(0).SubItems.Add("설정에서 원하는 학과를 선택해주세요.")
                listview(Tab_index).Items.Add("info")
                listview(Tab_index).Items(1).SubItems.Add("Please select the department you want in Settings.")
            Else 'URL이 존재할 경우
                Get_notice(Tab_index, Url, Url) '해당 리스트뷰에 내용을 담는 함수를 호출합니다.
            End If
        Next
    End Sub

    Private Sub Add_list(call_listview_num As Integer) '더보기를 클릭했을때 작동합니다.
        listview_page(call_listview_num) += 1 '해당리스트뷰에 보여주는 홈페이지의 페이지 수를 기록합니다.
        listview(call_listview_num).Items(listview(call_listview_num).Items.Count - 1).Remove() '리스트뷰의 More 더보기 항목을 삭제합니다. 
        Dim Url, Original_Url As String
        Dim Url_Num As Integer
        Url = ""
        Original_Url = ""
        Select Case call_listview_num
            Case 0 '대학공지 size 7
                Url_Num = 4691
                Original_Url = "http://www.seoultech.ac.kr/service/info/notice/"
            Case 1 '학사공지 size 5
                Url_Num = 6112
                Original_Url = "http://www.seoultech.ac.kr/service/info/matters/"
            Case 2 '장학공지 size 5
                Url_Num = 5233
                Original_Url = "http://www.seoultech.ac.kr/service/info/janghak/"
            Case 3 '대학원공지
                Url_Num = 56589
                Original_Url = "http://www.seoultech.ac.kr/service/info/graduate/"
            Case 4
                Original_Url = My.Settings.Custom1_Url
                Url_Num = Split(Split(listview(call_listview_num).Items(listview(call_listview_num).Items.Count - 1).SubItems(5).Text, "&bnum=")(1), "&bidx")(0) '해당 홈페이지의 게시판 고유번호를 파싱합니다.
            Case 5
                Original_Url = My.Settings.Custom2_Url
                Url_Num = Split(Split(listview(call_listview_num).Items(listview(call_listview_num).Items.Count - 1).SubItems(5).Text, "&bnum=")(1), "&bidx")(0)
            Case 6
                Original_Url = My.Settings.Custom3_Url
                Url_Num = Split(Split(listview(call_listview_num).Items(listview(call_listview_num).Items.Count - 1).SubItems(5).Text, "&bnum=")(1), "&bidx")(0)
        End Select

        If Not Original_Url = "" Then 'URL이 등록되어있다면 다음 페이지의 게시판 URL을 생성하고, 리스트뷰에 출력하는 함수를 호출합니다.
            Url = Original_Url & "?bidx=" & Url_Num & "&bnum=" & Url_Num & "&allboard=" & Split(Split(listview(call_listview_num).Items(listview(call_listview_num).Items.Count - 1).SubItems(5).Text, "allboard=")(1), "&nowpage")(0) & "&page=" & listview_page(call_listview_num) & "&size=5&searchtype=1&searchtext="
            Get_notice(call_listview_num, Url, Original_Url)
        End If
    End Sub
    Private Sub Get_notice(call_listview_num As Integer, Url As String, Original_Url As String) '리스트뷰의 내용을 추가합니다.(리스트뷰의 번호, 로딩할 페이지의 주소, 해당 게시판의 1페이지 주소)
        Dim loopstart, countnotice, counttotalnotice As Integer
        Dim temp As String

        Winhttp.Open("GET", Url)
        Winhttp.Send()
        countnotice = UBound(Split(Winhttp.ResponseText, "<img alt=""notice""")) + 1
        counttotalnotice = UBound(Split(Winhttp.ResponseText, "<tr class=""body_tr"">")) '전체

        If (listview(call_listview_num).Items.Count = 0) Then
            loopstart = 1
        Else
            loopstart = countnotice
        End If

        If call_listview_num <= 3 And Not counttotalnotice = 0 Then '학교홈페이지에 있는 게시판을 파싱합니다.
            For i = loopstart To counttotalnotice
                Dim Listview_index As Integer = listview(call_listview_num).Items.Count
                temp = Split(Split(Winhttp.ResponseText, "<tr class=""body_tr"">")(i), "</tr>")(0) 'temp에 공지사항을 받아옴.
                If (loopstart = 1 And i < countnotice) Then
                    listview(call_listview_num).Items.Add("공지")
                Else
                    listview(call_listview_num).Items.Add(Split(Split(temp, "<td style=""text-align: center;"">")(1), "</td>")(0))
                End If
                listview(call_listview_num).Items(Listview_index).SubItems.Add(Replace(Replace(Split(Split(temp, ">")(4 + UBound(Split(temp, "ico_notice_ko.gif"))), "<")(0), vbTab, ""), vbCrLf, ""))
                listview(call_listview_num).Items(Listview_index).SubItems.Add(Split(Split(temp, "<td style=""text-align: center;"">")(3), "</td>")(0)) '게시일자
                listview(call_listview_num).Items(Listview_index).SubItems.Add(Split(Split(temp, "<td style=""text-align: center;"">")(4), "</td>")(0)) '게시자
                listview(call_listview_num).Items(Listview_index).SubItems.Add(Original_Url)
                listview(call_listview_num).Items(Listview_index).SubItems.Add(Split(Split(temp, "<a href='")(1), "'>")(0))
            Next
        ElseIf call_listview_num > 3 And Not counttotalnotice = 0 Then '각 학과의 홈페이지를 파싱합니다.
            For i = loopstart To counttotalnotice
                Dim Listview_index As Integer = listview(call_listview_num).Items.Count
                temp = Split(Split(Winhttp.ResponseText, "<tr class=""body_tr"">")(i), "</tr>")(0) 'temp에 공지사항을 받아옴.
                If (loopstart = 1 And i < countnotice) Then
                    listview(call_listview_num).Items.Add("공지")
                Else
                    listview(call_listview_num).Items.Add(Split(Split(temp, "<td class=""body_col_number"">")(1), "</td>")(0))
                End If
                listview(call_listview_num).Items(Listview_index).SubItems.Add(Replace(Replace(Split(Split(temp, ">")(5 + UBound(Split(temp, "ico_notice_ko.gif"))), "<")(0), vbTab, ""), vbCrLf, ""))
                listview(call_listview_num).Items(Listview_index).SubItems.Add(Split(Split(temp, "<td class=""body_col_regdate"" align=""center"">")(1), "</td>")(0))
                listview(call_listview_num).Items(Listview_index).SubItems.Add(Split(Split(temp, "<td class=""body_col_user"" align=""center"">")(2), "</td>")(0))
                listview(call_listview_num).Items(Listview_index).SubItems.Add(Original_Url)
                listview(call_listview_num).Items(Listview_index).SubItems.Add(Split(Split(temp, "<a href='")(1), "'>")(0))
            Next
        Else
            MsgBox("공지사항을 로딩할 수 없습니다.")
        End If
        listview(call_listview_num).Items.Add("More") '파싱을 완료하고, 리스트뷰에 아이템을 등록완료한 후에 더보기 를 추가합니다
        listview(call_listview_num).Items(listview(call_listview_num).Items.Count - 1).SubItems.Add("더 많은 게시물 보기")
    End Sub
    '여기부터 7개는 해당 리스트뷰에서 더블클릭을 할 경우 어떤 행동을 할지 결정합니다.
    'URL이 없는경우(info)의 경우 아무런 반응을 하지 않습니다.
    '더보기(More)일 경우, 다시 함수를 호출하여 리스트뷰에 더 많은 항목을 불러옵니다.
    '일반적인 항목일경우 해당 게시물을 인터넷브라우저를 이용하여 실행합니다.
    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs) Handles ListView1.DoubleClick
        If ListView1.Items(ListView1.FocusedItem.Index).SubItems(0).Text = "info" Then
        ElseIf ListView1.Items(ListView1.FocusedItem.Index).SubItems(0).Text = "More" Then
            Add_list(0)
        Else
            Process.Start(ListView1.Items(ListView1.FocusedItem.Index).SubItems(4).Text & ListView1.Items(ListView1.FocusedItem.Index).SubItems(5).Text)
        End If
    End Sub
    Private Sub ListView2_DoubleClick(sender As Object, e As EventArgs) Handles ListView2.DoubleClick
        If ListView2.Items(ListView2.FocusedItem.Index).SubItems(0).Text = "info" Then
        ElseIf ListView2.Items(ListView2.FocusedItem.Index).SubItems(0).Text = "More" Then
            Add_list(1)
        Else
            Process.Start(ListView2.Items(ListView2.FocusedItem.Index).SubItems(4).Text & ListView2.Items(ListView2.FocusedItem.Index).SubItems(5).Text)
        End If
    End Sub
    Private Sub ListView3_DoubleClick(sender As Object, e As EventArgs) Handles ListView3.DoubleClick
        If ListView3.Items(ListView3.FocusedItem.Index).SubItems(0).Text = "info" Then
        ElseIf ListView3.Items(ListView3.FocusedItem.Index).SubItems(0).Text = "More" Then
            Add_list(2)
        Else
            Process.Start(ListView3.Items(ListView3.FocusedItem.Index).SubItems(4).Text & ListView3.Items(ListView3.FocusedItem.Index).SubItems(5).Text)
        End If
    End Sub
    Private Sub ListView4_DoubleClick(sender As Object, e As EventArgs) Handles ListView4.DoubleClick
        If ListView4.Items(ListView4.FocusedItem.Index).SubItems(0).Text = "info" Then
        ElseIf ListView4.Items(ListView4.FocusedItem.Index).SubItems(0).Text = "More" Then
            Add_list(3)
        Else
            Process.Start(ListView4.Items(ListView4.FocusedItem.Index).SubItems(4).Text & ListView4.Items(ListView4.FocusedItem.Index).SubItems(5).Text)
        End If
    End Sub
    Private Sub ListView5_DoubleClick(sender As Object, e As EventArgs) Handles ListView5.DoubleClick
        If ListView5.Items(ListView5.FocusedItem.Index).SubItems(0).Text = "info" Then
        ElseIf ListView5.Items(ListView5.FocusedItem.Index).SubItems(0).Text = "More" Then
            Add_list(4)
        Else
            Process.Start(ListView5.Items(ListView5.FocusedItem.Index).SubItems(4).Text & ListView5.Items(ListView5.FocusedItem.Index).SubItems(5).Text)
        End If
    End Sub
    Private Sub ListView6_DoubleClick(sender As Object, e As EventArgs) Handles ListView6.DoubleClick
        If ListView6.Items(ListView6.FocusedItem.Index).SubItems(0).Text = "info" Then
        ElseIf ListView6.Items(ListView6.FocusedItem.Index).SubItems(0).Text = "More" Then
            Add_list(5)
        Else
            Process.Start(ListView6.Items(ListView6.FocusedItem.Index).SubItems(4).Text & ListView6.Items(ListView6.FocusedItem.Index).SubItems(5).Text)
        End If
    End Sub
    Private Sub ListView7_DoubleClick(sender As Object, e As EventArgs) Handles ListView7.DoubleClick
        If ListView7.Items(ListView7.FocusedItem.Index).SubItems(0).Text = "info" Then
        ElseIf ListView7.Items(ListView7.FocusedItem.Index).SubItems(0).Text = "More" Then
            Add_list(6)
        Else
            Process.Start(ListView7.Items(ListView7.FocusedItem.Index).SubItems(4).Text & ListView7.Items(ListView7.FocusedItem.Index).SubItems(5).Text)
        End If
    End Sub
    Private Sub ChromeButton1_Click(sender As Object, e As EventArgs) Handles ChromeButton1.Click '저장버튼을 누를경우
        My.Settings.Custom1_Name = ComboBox1.Text '해당학과의 이름과 URL을 저장합니다.
        My.Settings.Custom1_Url = Search_Url(ComboBox1.SelectedIndex)
        My.Settings.Custom2_Name = ComboBox2.Text
        My.Settings.Custom2_Url = Search_Url(ComboBox2.SelectedIndex)
        My.Settings.Custom3_Name = ComboBox3.Text
        My.Settings.Custom3_Url = Search_Url(ComboBox3.SelectedIndex)

        Check_Tab_Name()

        Dim Url As String = ""
        For Tab_index = 4 To 6  '저장이 끝난후 다시 탭의 이름을 교체하고 리스트뷰를 로딩합니다.
            listview(Tab_index).Items.Clear()
            Select Case Tab_index
                Case 4
                    Url = My.Settings.Custom1_Url
                    listview_page(4) = 1
                Case 5
                    Url = My.Settings.Custom2_Url
                    listview_page(5) = 1

                Case 6
                    Url = My.Settings.Custom3_Url
                    listview_page(6) = 1

            End Select

            If Url = "" Then
                listview(Tab_index).Items.Add("info")
                listview(Tab_index).Items(0).SubItems.Add("설정에서 원하는 학과를 선택해주세요.")
                listview(Tab_index).Items.Add("info")
                listview(Tab_index).Items(1).SubItems.Add("Please select the department you want in Settings.")
            Else
                Get_notice(Tab_index, Url, Url)
            End If
        Next
        MsgBox("저장되었습니다.")
    End Sub
    Function Search_Url(Seatch_index As Integer)
        Dim Url As String
        Select Case Seatch_index
            Case 0  '공과대학
                Url = "http://engineer.seoultech.ac.kr/community/notice/"
            Case 1
                Url = "http://msd.seoultech.ac.kr/information/bulletin/"
            Case 2
                Url = "http://mae.seoultech.ac.kr/bachelor/bulletin/"
            Case 3
                Url = "http://safety.seoultech.ac.kr/b_information/notic/"
            Case 4
                Url = "http://mse.seoultech.ac.kr/b_intro/notice/"
            Case 5
                Url = "http://inarc.seoultech.ac.kr/ba/news/"
            Case 6
                Url = "http://mee.seoultech.ac.kr/info/news/"
            Case 7
                Url = "http://fm.seoultech.ac.kr/info/news/"
            Case 8
                Url = "http://civil.seoultech.ac.kr/haksainfor/bulletin/"
            Case 9
                Url = "http://ebse.seoultech.ac.kr/info/notice/"
            Case 10
                Url = "http://architecture.seoultech.ac.kr/bachelor_of_information/notice/"
            Case 11
                Url = "http://archidesign.seoultech.ac.kr/bachelor_information/notice/"
            Case 12  '정통대
                Url = "http://ice.seoultech.ac.kr/community/notice/bulletin/"
            Case 13
                Url = "http://eie.seoultech.ac.kr/majornotice/notice/"
            Case 14
                Url = "http://eeme.seoultech.ac.kr/bachelor_info/notice/"
            Case 15  '컴퓨터공학과
                Url = "http://computer.seoultech.ac.kr/info_plaza/notic/"
            Case 16  '에바대
                Url = "http://nature.seoultech.ac.kr/community/notic/"
            Case 17
                Url = "http://che.seoultech.ac.kr/engineering_certification/board/"
            Case 18
                Url = "http://enviro.seoultech.ac.kr/information/notice/"
            Case 19
                Url = "http://food.seoultech.ac.kr/bachelor/notice/"
            Case 20
                Url = "http://fchem.seoultech.ac.kr/bachelor_of_information_/notice_/"
            Case 21
                Url = "http://sports.seoultech.ac.kr/b_information/notic/"
            Case 22
                Url = "http://optometry.seoultech.ac.kr/bachelor_of_information/notice/"
            Case 23  '조형대학
                Url = "http://and.seoultech.ac.kr/advertisements/notice/"
            Case 24
                Url = "http://design.seoultech.ac.kr/square/bulletin/"
            Case 25
                Url = "http://metalartdesign.seoultech.ac.kr/b_information/notic/"
            Case 26
                Url = "http://fineart.seoultech.ac.kr/community/notice/"
            Case 27  '인사대
                Url = "http://human.seoultech.ac.kr/community/notice/"
            Case 28
                Url = "http://eng.seoultech.ac.kr/information_plaza/notice/"
            Case 29
                Url = "http://pa.seoultech.ac.kr/admini/notice/"
            Case 30
                Url = "http://writing-creative.seoultech.ac.kr/bachelor_of_information_/notice_/"
            Case 31
                Url = "http://liberal.seoultech.ac.kr/community/notic/"
            Case 32  '기술경영융합
                Url = "http://sgc.seoultech.ac.kr/open/notice/"
            Case 33
                Url = "http://iise.seoultech.ac.kr/notice/faculty_announcements_/"
            Case 34
                Url = "http://msde.seoultech.ac.kr/academics_/qna/"
            Case 35
                Url = "http://itm.seoultech.ac.kr/bachelor_of_information/notice/"
            Case 36
                Url = "http://biz.seoultech.ac.kr/info_plaza/notic/"
            Case 37
                Url = "http://gtm.seoultech.ac.kr/info_plaza/gtm_info/"
            Case 38
                Url = "http://m-disciplinary.seoultech.ac.kr/community/notice/"
            Case 39
                Url = "http://mc.seoultech.ac.kr/community/notice/"
            Case 40
                Url = "http://sssf.seoultech.ac.kr/community/notice/"
            Case 41
                Url = "http://icee.seoultech.ac.kr/bbs/notice/"
            Case Else
                Url = ""
        End Select

        Return Url
    End Function '콤보박스의 선택된 인덱스를 이용하여 해당학과의 URL을 리턴합니다.

End Class

using DevExpress.Utils;
using DevExpress.XtraEditors;
using System;
using System.Threading;
using System.Windows.Forms;

namespace WindowsCAssistant
{
    public partial class frmNewForm : XtraForm
    {
        #region Attributes
        string VSCodePath = Configuration.Default.VisualStudioCodePath, DOSBoxPath = Tools.DosBoxConfigPath, FolderPath = Configuration.Default.Directory;
        Archive NewForm = new Archive();
        int WidthRecommended = 132;
        bool EditStage = false;
        #endregion

        public frmNewForm()
        {
            InitializeComponent();
        }

        #region Methods
        private void CreateCode()
        {
            if (rgExtension.EditValue.ToString() == "H")
                NewForm.Content = $@"/*  */
typedef struct
{{
    FormProperties Properties;
    /*/Buttons/*/
    bool Close[2];
    bool Minimize[2];
    bool Maximize[2];
}} {txtName.Text}Class;

void form{txtName.Text}_Icon(int x, int y);
void form{txtName.Text}_Init(void);
void form{txtName.Text}_Designer(void);
void form{txtName.Text}_PreEvent(void);
void form{txtName.Text}_Event(void);
void form{txtName.Text}_PostEvent(void);

{txtName.Text}Class {txtName.Text};

void form{txtName.Text}_Init(void)
{{
    {txtName.Text}.Properties.Location[0] = 10;
    {txtName.Text}.Properties.Location[1] = 10;
    {txtName.Text}.Properties.Size[0] = {Convert.ToInt32(txtX.EditValue)};
    {txtName.Text}.Properties.Size[1] = {Convert.ToInt32(txtY.EditValue)};
    {txtName.Text}.Properties.Text = ""{txtTitle.Text}"";
    {txtName.Text}.Properties.BackColor = {icbeBackground.EditValue};
    {txtName.Text}.Properties.Visible = true;
    {txtName.Text}.Properties.WindowState = NormalState;
}}

void form{txtName.Text}_Designer(void)
{{
    int Location[2], Size[2];
    memcpy(Location, {txtName.Text}.Properties.Location, sizeof(Location));
    memcpy(Size, {txtName.Text}.Properties.Size, sizeof(Size));

    /*---------------------------------------
            Form
    ----------------------------------------*/
    setfillstyle(SOLID_FILL, {txtName.Text}.Properties.BackColor);
    bar(Location[0], Location[1] + 20, Size[0] + Location[0], Size[1] + Location[1]);

    setfillstyle(SOLID_FILL, WHITE);
    bar(Location[0], Location[1], Size[0] + Location[0], Location[1] + 20);

    /*---------------------------------------
            Command bar
    ----------------------------------------*/

    /*----------- Title --------------------*/
    setcolor(BLACK);
    setfillstyle(SOLID_FILL, BLACK);
    settextstyle(6, HORIZ_DIR, 1);
    setcolor(BLACK);
    outtextxy(Location[0] + 25, Location[1] - 5, {txtName.Text}.Properties.Text);

    form{txtName.Text}_Icon(Location[0] + 1, Location[1] + 1);

    /*----------- Close button --------------------*
    setfillstyle(SOLID_FILL,RED);
    setcolor(WHITE);
    bar(Location[0] + (Size[0] - 35),Location[1],Size[0] + Location[0],Location[1] + 20);/**/
    outtextxy(Location[0] + (Size[0] - 20), Location[1] - 7, ""x"");

    /*----------- Maximize button --------------------*
    bar(Location[0] + (Size[0] - 70),Location[1],Size[0] - 35 + Location[0],Location[1] + 20);/**/
    rectangle(Location[0] + (Size[0] - 70) + 14, Location[1] + 7, Location[0] + (Size[0] - 70) + 21, Location[1] + 13);

    /*----------- Minimize button --------------------*
    bar(Location[0] + (Size[0] - 105),Location[1],Size[0] - 70 + Location[0],Location[1] + 20);/**/
    line(Location[0] + (Size[0] - 105) + 14, Location[1] + 10, Location[0] + (Size[0] - 105) + 21, Location[1] + 10);

    /*----------- Edge --------------------*/
    rectangle(Location[0], Location[1], Size[0] + Location[0], Size[1] + Location[1]);
}}

void form{txtName.Text}_PreEvent(void)
{{
    int Location[2], Size[2];
    memcpy(Location, {txtName.Text}.Properties.Location, sizeof(Location));
    memcpy(Size, {txtName.Text}.Properties.Size, sizeof(Size));
    /*---------------------------------------
            Detection 
    ----------------------------------------*/
    if (mxpos(1) > Location[0] + (Size[0] - 35) && mypos(1) > Location[1] && mxpos(1) < Size[0] + Location[0] && mypos(1) < Location[1] + 20)
        {txtName.Text}.Close[0] = true;
    else
        {txtName.Text}.Close[0] = false;

    if (mxpos(1) > Location[0] + (Size[0] - 70) && mypos(1) > Location[1] && mxpos(1) < Size[0] - 35 + Location[0] && mypos(1) < Location[1] + 20)
        {txtName.Text}.Maximize[0] = true;
    else
        {txtName.Text}.Maximize[0] = false;

    if (mxpos(1) > Location[0] + (Size[0] - 105) && mypos(1) > Location[1] && mxpos(1) < Size[0] - 70 + Location[0] && mypos(1) < Location[1] + 20)
        {txtName.Text}.Minimize[0] = true;
    else
        {txtName.Text}.Minimize[0] = false;

    if ({txtName.Text}.Close[0] != {txtName.Text}.Close[1])
    {{
        mocultar();
        setfillstyle(SOLID_FILL, {txtName.Text}.Close[0] == true ? RED : WHITE);
        setcolor({txtName.Text}.Close[0] == true ? WHITE : BLACK);
        bar(Location[0] + (Size[0] - 35), Location[1] + 1, Size[0] + Location[0] - 1, Location[1] + 20);
        outtextxy(Location[0] + (Size[0] - 20), Location[1] - 7, ""x"");
        mver();
        {txtName.Text}.Close[1] = {txtName.Text}.Close[0];
    }}

    if ({txtName.Text}.Maximize[0] != {txtName.Text}.Maximize[1])
    {{
        mocultar();
        setfillstyle(SOLID_FILL, {txtName.Text}.Maximize[0] == true ? LIGHTGRAY : WHITE);
        bar(Location[0] + (Size[0] - 70), Location[1] + 1, Size[0] - 35 + Location[0], Location[1] + 20); 
        setcolor(BLACK);
        rectangle(Location[0] + (Size[0] - 70) + 14, Location[1] + 7, Location[0] + (Size[0] - 70) + 21, Location[1] + 13);
        mver();
        {txtName.Text}.Maximize[1] = {txtName.Text}.Maximize[0];
    }}

    if ({txtName.Text}.Minimize[0] != {txtName.Text}.Minimize[1])
    {{
        mocultar();
        setfillstyle(SOLID_FILL, {txtName.Text}.Minimize[0] == true ? LIGHTGRAY : WHITE);
        bar(Location[0] + (Size[0] - 105), Location[1] + 1, Size[0] - 70 + Location[0], Location[1] + 20); /**/
        setcolor(BLACK);
        line(Location[0] + (Size[0] - 105) + 14, Location[1] + 10, Location[0] + (Size[0] - 105) + 21, Location[1] + 10);
        mver();
        {txtName.Text}.Minimize[1] = {txtName.Text}.Minimize[0];
    }}
}}

void form{txtName.Text}_Event(void)
{{
    int initLocation[2];
    int tempLocation[2];
    int mouse[2];

    if (mxpos(1) > {txtName.Text}.Properties.Location[0] && mypos(1) > {txtName.Text}.Properties.Location[1] && mxpos(1) < {txtName.Text}.Properties.Size[0] + {txtName.Text}.Properties.Location[0] - 105 && mypos(1) < 20 + {txtName.Text}.Properties.Location[1] && mclick() == 1)
    {{
        mouse[0] = mxpos(1);
        mouse[1] = mypos(1);
        do
        {{
            tempLocation[0] = {txtName.Text}.Properties.Location[0];
            tempLocation[1] = {txtName.Text}.Properties.Location[1];
            {txtName.Text}.Properties.Location[0] -= mouse[0] - mxpos(1);
            {txtName.Text}.Properties.Location[1] -= mouse[1] - mypos(1);
            mouse[0] = mxpos(1);
            mouse[1] = mypos(1);

            if (tempLocation[0] != {txtName.Text}.Properties.Location[0] || tempLocation[1] != {txtName.Text}.Properties.Location[1])
            {{
                mocultar();
                setfillstyle(SOLID_FILL, CYAN);
                bar(tempLocation[0], tempLocation[1], tempLocation[0] + {txtName.Text}.Properties.Size[0], tempLocation[1] + {txtName.Text}.Properties.Size[1]);
                form{txtName.Text}_Designer();
                mver();
                delay(1800);
            }}
        }} while (mclick() != 0);
    }}
}}

void form{txtName.Text}_PostEvent(void)
{{
    int Location[2], Size[2];

    memcpy(Location, {txtName.Text}.Properties.Location, sizeof(Location));
    memcpy(Size, {txtName.Text}.Properties.Size, sizeof(Size));

    if (mxpos(1) > Location[0] + (Size[0] - 35) && mypos(1) > Location[1] && mxpos(1) < Size[0] + Location[0] && mypos(1) < Location[1] + 20)
    {{
        mocultar();
        /*ClearScreen();*/
        {txtName.Text}.Properties.Visible = false;
        mver();
    }}
    else if (mxpos(1) > Location[0] + (Size[0] - 70) && mypos(1) > Location[1] && mxpos(1) < Size[0] - 35 + Location[0] && mypos(1) < Location[1] + 20)
    {{
        if ({txtName.Text}.Properties.WindowState == NormalState)
        {{
            {txtName.Text}.Properties.Location[0] = 0;
            {txtName.Text}.Properties.Location[1] = 0;
            {txtName.Text}.Properties.Size[0] = 640;
            {txtName.Text}.Properties.Size[1] = 480;
            {txtName.Text}.Properties.WindowState = MaximizeState;
        }}
        else if ({txtName.Text}.Properties.WindowState == MaximizeState)
        {{
            mocultar();
            /*ClearScreen();*/
            mver();
            {txtName.Text}.Properties.Location[0] = 20;
            {txtName.Text}.Properties.Location[1] = 20;
            {txtName.Text}.Properties.Size[0] = 300;
            {txtName.Text}.Properties.Size[1] = 200;
            {txtName.Text}.Properties.WindowState = NormalState;
        }}

        mocultar();
        form{txtName.Text}_Designer();
        mver();
    }}
    else if (mxpos(1) > Location[0] + (Size[0] - 105) && mypos(1) > Location[1] && mxpos(1) < Size[0] - 70 + Location[0] && mypos(1) < Location[1] + 20)
    {{
        /* Codigo para minimizar */
    }}
}}

void form{txtName.Text}_Icon(int x, int y)
{{
    bar(5 + x, 5 + y, 14 + x, 14 + y);
    bar(9 + x, 16 + y, 10 + x, 18 + y);
    bar(16 + x, 9 + y, 18 + x, 10 + y);
    bar(9 + x, 3 + y, 10 + x, 1 + y);
    bar(3 + x, 9 + y, 1 + x, 10 + y);

    line(15 + x, 12 + y, 15 + x, 7 + y);
    line(12 + x, 15 + y, 7 + x, 15 + y);
    line(4 + x, 7 + y, 4 + x, 12 + y);
    line(7 + x, 4 + y, 12 + x, 4 + y);

    putpixel(4 + x, 4 + y, BLACK);
    putpixel(4 + x, 15 + y, BLACK);
    putpixel(15 + x, 15 + y, BLACK);
    putpixel(15 + x, 4 + y, BLACK);

    setfillstyle(SOLID_FILL, WHITE == getcolor() ? BLACK : WHITE);
    bar(7 + x, 7 + y, 8 + x, 8 + y);
}}";

            else
                NewForm.Content = $@"#include <graphics.h>
#include <conio.h>
#include <stdlib.h>
#include <mouse.h>
#include <string.h>
#include <stdbool.h>

enum WindowState
{{
    CloseState,
    NormalState,
    MinimizeState,
    MaximizeState
}};

typedef struct 
{{
    int Size[2];
    int Location[2];
    char *Text;
    int BackColor;
    int WindowState;
    bool Visible;
}} FormProperties;

typedef struct
{{
    FormProperties Properties;
    /*/Buttons/*/
    bool Close[2];
    bool Minimize[2];
    bool Maximize[2];
}} {txtName.Text}Class;

typedef struct
{{
    int Background;
}} SystemProperties;

void {txtName.Text}_Icon(int x, int y);
void {txtName.Text}_Init(void);
void {txtName.Text}_Designer(void);
void {txtName.Text}_PreEvent(void);
void {txtName.Text}_Event(void);
void {txtName.Text}_PostEvent(void);

{txtName.Text}Class {txtName.Text};
SystemProperties System;

void main()
{{
    int mode = DETECT, gmode;
    initgraph(&mode, &gmode, """");

    

    System.Background = CYAN;
    setfillstyle(SOLID_FILL,System.Background);
    bar(0,0,640,480);

    {txtName.Text}_Init();
    {txtName.Text}_Designer();

    mver();

    do
    {{
        do
            if ({txtName.Text}.Properties.Visible)
            {{
                {txtName.Text}_PreEvent();
            }}
                
        while (mclick() == 0);

        if ({txtName.Text}.Properties.Visible)
            {{
                {txtName.Text}_Event();
            }}

        while (mclick() != 0);

        if ({txtName.Text}.Properties.Visible);
        {{
            {txtName.Text}_PostEvent();
        }}
    }} while ({txtName.Text}.Properties.Visible);
    getch();
}}
/**/
void ClearScreen(void)
{{
    setfillstyle(SOLID_FILL, System.Background);
    bar(0, 0, 640, 480);
}}

void {txtName.Text}_Init(void)
{{
    {txtName.Text}.Properties.Location[0] = 10;
    {txtName.Text}.Properties.Location[1] = 10;
    {txtName.Text}.Properties.Size[0] = {txtX.Text};
    {txtName.Text}.Properties.Size[1] = {txtY.Text};
    {txtName.Text}.Properties.Text = ""{txtTitle.Text}"";
    {txtName.Text}.Properties.BackColor = {icbeBackground.EditValue};
    {txtName.Text}.Properties.Visible = true;
    {txtName.Text}.Properties.WindowState = NormalState;
}}

void {txtName.Text}_Designer(void)
{{
    int Location[2], Size[2];
    memcpy(Location, {txtName.Text}.Properties.Location, sizeof(Location));
    memcpy(Size, {txtName.Text}.Properties.Size, sizeof(Size));

    /*---------------------------------------
            Form
    ----------------------------------------*/
    setfillstyle(SOLID_FILL, {txtName.Text}.Properties.BackColor);
    bar(Location[0], Location[1] + 20, Size[0] + Location[0], Size[1] + Location[1]);

    setfillstyle(SOLID_FILL, WHITE);
    bar(Location[0], Location[1], Size[0] + Location[0], Location[1] + 20);

    /*---------------------------------------
            Command bar
    ----------------------------------------*/

    /*----------- Title --------------------*/
    setcolor(BLACK);
    setfillstyle(SOLID_FILL, BLACK);
    settextstyle(6, HORIZ_DIR, 1);
    setcolor(BLACK);
    outtextxy(Location[0] + 25, Location[1] - 5, {txtName.Text}.Properties.Text);

    {txtName.Text}_Icon(Location[0] + 1, Location[1] + 1);

    /*----------- Close button --------------------*
    setfillstyle(SOLID_FILL,RED);
    setcolor(WHITE);
    bar(Location[0] + (Size[0] - 35),Location[1],Size[0] + Location[0],Location[1] + 20);*/
    outtextxy(Location[0] + (Size[0] - 20), Location[1] - 7, ""x"");

    /*----------- Maximize button --------------------*
    bar(Location[0] + (Size[0] - 70),Location[1],Size[0] - 35 + Location[0],Location[1] + 20);*/
    rectangle(Location[0] + (Size[0] - 70) + 14, Location[1] + 7, Location[0] + (Size[0] - 70) + 21, Location[1] + 13);

    /*----------- Minimize button --------------------*
    bar(Location[0] + (Size[0] - 105),Location[1],Size[0] - 70 + Location[0],Location[1] + 20);*/
    line(Location[0] + (Size[0] - 105) + 14, Location[1] + 10, Location[0] + (Size[0] - 105) + 21, Location[1] + 10);

    /*----------- Edge --------------------*/
    setcolor(BLACK);
    rectangle(Location[0], Location[1], Size[0] + Location[0], Size[1] + Location[1]);
}}

void {txtName.Text}_PreEvent(void)
{{
    int Location[2], Size[2];
    memcpy(Location, {txtName.Text}.Properties.Location, sizeof(Location));
    memcpy(Size, {txtName.Text}.Properties.Size, sizeof(Size));
    /*---------------------------------------
            Detection 
    ----------------------------------------*/
    if (mxpos(1) > Location[0] + (Size[0] - 35) && mypos(1) > Location[1] && mxpos(1) < Size[0] + Location[0] && mypos(1) < Location[1] + 20)
        {txtName.Text}.Close[0] = true;
    else
        {txtName.Text}.Close[0] = false;

    if (mxpos(1) > Location[0] + (Size[0] - 70) && mypos(1) > Location[1] && mxpos(1) < Size[0] - 35 + Location[0] && mypos(1) < Location[1] + 20)
        {txtName.Text}.Maximize[0] = true;
    else
        {txtName.Text}.Maximize[0] = false;

    if (mxpos(1) > Location[0] + (Size[0] - 105) && mypos(1) > Location[1] && mxpos(1) < Size[0] - 70 + Location[0] && mypos(1) < Location[1] + 20)
        {txtName.Text}.Minimize[0] = true;
    else
        {txtName.Text}.Minimize[0] = false;

    if ({txtName.Text}.Close[0] != {txtName.Text}.Close[1])
    {{
        mocultar();
        setfillstyle(SOLID_FILL, {txtName.Text}.Close[0] == true ? RED : WHITE);
        setcolor({txtName.Text}.Close[0] == true ? WHITE : BLACK);
        bar(Location[0] + (Size[0] - 35), Location[1] + 1, Size[0] + Location[0] - 1, Location[1] + 20); 
        outtextxy(Location[0] + (Size[0] - 20), Location[1] - 7, ""x"");
        mver();
        {txtName.Text}.Close[1] = {txtName.Text}.Close[0];
    }}

    if ({txtName.Text}.Maximize[0] != {txtName.Text}.Maximize[1])
    {{
        mocultar();
        setfillstyle(SOLID_FILL, {txtName.Text}.Maximize[0] == true ? LIGHTGRAY : WHITE);
        bar(Location[0] + (Size[0] - 70), Location[1] + 1, Size[0] - 35 + Location[0], Location[1] + 20); 
        setcolor(BLACK);
        rectangle(Location[0] + (Size[0] - 70) + 14, Location[1] + 7, Location[0] + (Size[0] - 70) + 21, Location[1] + 13);
        mver();
        {txtName.Text}.Maximize[1] = {txtName.Text}.Maximize[0];
    }}

    if ({txtName.Text}.Minimize[0] != {txtName.Text}.Minimize[1])
    {{
        mocultar();
        setfillstyle(SOLID_FILL, {txtName.Text}.Minimize[0] == true ? LIGHTGRAY : WHITE);
        bar(Location[0] + (Size[0] - 105), Location[1] + 1, Size[0] - 70 + Location[0], Location[1] + 20); 
        setcolor(BLACK);
        line(Location[0] + (Size[0] - 105) + 14, Location[1] + 10, Location[0] + (Size[0] - 105) + 21, {txtName.Text}.Location[1] + 10);
        mver();
        {txtName.Text}.Minimize[1] = {txtName.Text}.Minimize[0];
    }}
}}

void {txtName.Text}_Event(void)
{{
    int initLocation[2];
    int tempLocation[2];
    int mouse[2];
    
    if (mxpos(1) > {txtName.Text}.Properties.Location[0] && mypos(1) > {txtName.Text}.Properties.Location[1] && mxpos(1) < {txtName.Text}.Properties.Size[0] + {txtName.Text}.Properties.Location[0] - 105 && mypos(1) < 20 + {txtName.Text}.Properties.Location[1] && mclick() == 1)
    {{
        mouse[0] = mxpos(1);
        mouse[1] = mypos(1);
        do
        {{
            tempLocation[0] = {txtName.Text}.Properties.Location[0];
            tempLocation[1] = {txtName.Text}.Properties.Location[1];
            {txtName.Text}.Properties.Location[0] -= mouse[0] - mxpos(1);
            {txtName.Text}.Properties.Location[1] -= mouse[1] - mypos(1);
            mouse[0] = mxpos(1);
            mouse[1] = mypos(1);

            if (tempLocation[0] != {txtName.Text}.Properties.Location[0] || tempLocation[1] != {txtName.Text}.Properties.Location[1])
            {{
                mocultar();
                setfillstyle(SOLID_FILL, System.Background);
                bar(tempLocation[0], tempLocation[1], tempLocation[0] + {txtName.Text}.Properties.Size[0], tempLocation[1] + {txtName.Text}.Properties.Size[1]);
                {txtName.Text}_Designer();
                mver();
                delay(1800);
            }}
        }} while (mclick() != 0);
    }}
    
}}

void {txtName.Text}_PostEvent(void)
{{
    int Location[2], Size[2];

    memcpy(Location, {txtName.Text}.Properties.Location, sizeof(Location));
    memcpy(Size, {txtName.Text}.Properties.Size, sizeof(Size));

    if (mxpos(1) > Location[0] + (Size[0] - 35) && mypos(1) > Location[1] && mxpos(1) < Size[0] + Location[0] && mypos(1) < Location[1] + 20)
    {{
        mocultar();
        ClearScreen();
        {txtName.Text}.Properties.Visible = false;
        mver();
    }}
    else if (mxpos(1) > Location[0] + (Size[0] - 70) && mypos(1) > Location[1] && mxpos(1) < Size[0] - 35 + Location[0] && mypos(1) < Location[1] + 20)
    {{
        if ({txtName.Text}.Properties.WindowState == NormalState)
        {{
            {txtName.Text}.Properties.Location[0] = 0;
            {txtName.Text}.Properties.Location[1] = 0;
            {txtName.Text}.Properties.Size[0] = 640;
            {txtName.Text}.Properties.Size[1] = 480;
            {txtName.Text}.Properties.WindowState = MaximizeState;
        }}
        else if ({txtName.Text}.Properties.WindowState == MaximizeState)
        {{
            mocultar();
            ClearScreen();
            mver();
            {txtName.Text}.Properties.Location[0] = 20;
            {txtName.Text}.Properties.Location[1] = 20;
            {txtName.Text}.Properties.Size[0] = 300;
            {txtName.Text}.Properties.Size[1] = 200;
            {txtName.Text}.Properties.WindowState = NormalState;
        }}

        mocultar();
        {txtName.Text}_Designer();
        mver();
    }}
    else if (mxpos(1) > Location[0] + (Size[0] - 105) && mypos(1) > Location[1] && mxpos(1) < Size[0] - 70 + Location[0] && mypos(1) < Location[1] + 20)
    {{
    }}
}}

void {txtName.Text}_Icon(int x, int y)
{{
    bar(5 + x, 5 + y, 14 + x, 14 + y);
    bar(9 + x, 16 + y, 10 + x, 18 + y);
    bar(16 + x, 9 + y, 18 + x, 10 + y);
    bar(9 + x, 3 + y, 10 + x, 1 + y);
    bar(3 + x, 9 + y, 1 + x, 10 + y);

    line(15 + x, 12 + y, 15 + x, 7 + y);
    line(12 + x, 15 + y, 7 + x, 15 + y);
    line(4 + x, 7 + y, 4 + x, 12 + y);
    line(7 + x, 4 + y, 12 + x, 4 + y);

    putpixel(4 + x, 4 + y, BLACK);
    putpixel(4 + x, 15 + y, BLACK);
    putpixel(15 + x, 15 + y, BLACK);
    putpixel(15 + x, 4 + y, BLACK);

    setfillstyle(SOLID_FILL, WHITE == getcolor() ? BLACK : WHITE);
    bar(7 + x, 7 + y, 8 + x, 8 + y);
}}";

            meCode.Text = NewForm.Content;
        }

        private bool ValidateInfo()
        {
            if (txtName.Text == string.Empty)
                XtraMessageBox.Show("Debe escribir un nombre al formulario", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            else if (txtTitle.Text == string.Empty)
                XtraMessageBox.Show("Debe escribir un título al formulario", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            else if (txtX.Text == string.Empty)
                XtraMessageBox.Show("Debe escribir un valor de tamaño en x", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            else if (txtY.Text == string.Empty)
                XtraMessageBox.Show("Debe escoger un valor de tamaño en y", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            else if (icbeBackground.EditValue == null)
                XtraMessageBox.Show("Debe escoger un color de fondo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            else if (rgExtension.EditValue == null)
                XtraMessageBox.Show("Debe escoger el tipo de código", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            else
                return true;

            return false;
        }

        private void EnableControls(bool Enabled = true)
        {
            txtName.Enabled = Enabled;
            txtTitle.Enabled = Enabled;
            txtX.Enabled = Enabled;
            txtY.Enabled = Enabled;
            icbeBackground.Enabled = Enabled;

            meCode.Enabled = !Enabled;
        }

        private void SaveCode(WaitDialogForm waitdialogform)
        {
            CheckInclude();

            if ((char)rgExtension.EditValue == 'C')
                NewForm.Path = $@"{FolderPath}\{txtName.Text}.c";
            else
                NewForm.Path = $@"c:\TC20\Include\{txtName.Text}.h";

            NewForm.Content = meCode.Text;

            if (NewForm.Exist())
                if (XtraMessageBox.Show(Tools.Message.Overwrite, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

            NewForm.Save();
            waitdialogform.Close();

            if (ceVSCode.Checked)
                Tools.ExecuteCommand($@"""{VSCodePath}"" ""{NewForm.Path}""");

            if (ceDOSBox.Checked && (char)rgExtension.EditValue != 'H')
            {
                Tools.OpenCFile($"{txtName.Text}.c", FolderPath, false);
                Tools.openDOSBox();
            }
            else if(ceExecute.Checked && (char)rgExtension.EditValue != 'H')
            {
                Tools.OpenCFile($"{txtName.Text}.c", FolderPath, true);
                Tools.openDOSBox();
            }

            
            XtraMessageBox.Show(Tools.Message.GenerateSuccess, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private void CheckInclude()
        {
            Archive form = new Archive(@"C:\TC20\include\FORM.H");
            Archive stdbool = new Archive(@"C:\TC20\include\stdbool.h");
            if(!stdbool.Exist())
            {
                stdbool.Content = @"#ifndef STDBOOL_H
#define STDBOOL_h
typedef enum {false=0, true} bool;
#endif";
                stdbool.Save();
            }
            if(!form.Exist())
            {
                form.Content = @"#include <stdbool.h>
enum WindowState
{
    CloseState,
    NormalState,
    MinimizeState,
    MaximizeState
};

typedef struct 
{
    int Size[2];
    int Location[2];
    char *Text;
    int BackColor;
    int WindowState;
    bool Visible;
} FormProperties;

typedef struct
{
    int Background;
} SystemProperties;";
                form.Save();
            }
        }
        #endregion

        #region Events
        private void frmNewForm_Load(object sender, EventArgs e)
        {
            try
            {
                EnableControls();
            }
            catch
            {
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBehind_Click(object sender, EventArgs e)
        {
            try
            {
                if (!EditStage)
                    Close();
                else
                {
                    meCode.Text = string.Empty;
                    EnableControls(true);
                    EditStage = false;
                    btnNext.Text = "Siguiente";
                }
            }
            catch
            {
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
         
        private void btnNext_Click(object sender, EventArgs e)
        {
            WaitDialogForm waitdialogform = new WaitDialogForm("Espere un momento...", "Generando código");
            try
            {
                if (!EditStage && ValidateInfo())
                {
                    CreateCode();
                    EnableControls(false);
                    EditStage = true;
                    btnNext.Text = "Generar código";
                }
                else if (EditStage)
                {
                    SaveCode(waitdialogform);
                }
                waitdialogform.Close();
            }
            catch
            {
                if(waitdialogform.Enabled)
                    waitdialogform.Close();   
                MessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTitle_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                WidthRecommended = 130 + (txtTitle.Text.Length * 8);
                txtX.Text = WidthRecommended.ToString();
            }
            catch
            {
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtX_Leave(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(txtX.Text) < WidthRecommended)
                    txtX.Text = WidthRecommended.ToString();
            }
            catch
            {
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtY_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(txtY.Text) < 30)
                    txtY.Text = "30";
            }
            catch
            {
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ceDOSBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if ((char)rgExtension.EditValue == 'H' && ceDOSBox.Checked)
                {
                    XtraMessageBox.Show("Para abrir el archivo en DOSBox debe ser extensión C", "Abrir en DosBox", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ceDOSBox.Checked = false;
                }
                else if (ceDOSBox.Checked && ceExecute.Checked)
                    ceExecute.Checked = false;
            }
            catch
            {
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ceExecute_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if ((char)rgExtension.EditValue == 'H' && ceExecute.Checked)
                {
                    XtraMessageBox.Show("Para ejecutar el archivo en DOSBox debe ser extensión C", "Abrir en DosBox", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ceExecute.Checked = false;
                }
                else if(ceDOSBox.Checked && ceExecute.Checked)
                    ceDOSBox.Checked = false;
            }
            catch
            {
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rgExtension_Modified(object sender, EventArgs e)
        {
            try
            {
                if ((char)rgExtension.EditValue == 'H')
                    ceDOSBox.Checked = false;
            }
            catch
            {
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
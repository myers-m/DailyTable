using System.Collections;
using System.Collections.Generic;
using OfficeOpenXml;
using UnityEngine;

public class Excel {
    public List<ExcelWorksheet> Tables = new List<ExcelWorksheet> ();

    public Excel () {

    }

    public Excel (ExcelWorkbook wb) {
        for (int i = 0; i < wb.Worksheets.Count; i++) {
            ExcelWorksheet sheet = wb.Worksheets[i + 1];
            Tables.Add (sheet);
        }
    }

}
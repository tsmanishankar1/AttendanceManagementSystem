using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxCardHolderTest
{
    public decimal ChCardNo { get; set; }

    public string ChEmpId { get; set; } = null!;

    public string? ChTitle { get; set; }

    public string ChFname { get; set; } = null!;

    public string? ChLname { get; set; }

    public string? ChShortName { get; set; }

    public DateTime? ChDob { get; set; }

    public string? ChGender { get; set; }

    public string? ChNationality { get; set; }

    public string? ChContactAddress { get; set; }

    public string? ChPhoneNumber { get; set; }

    public string? ChMailId { get; set; }

    public int? ChDnId { get; set; }

    public int? ChCtId { get; set; }

    public int? ChCgId { get; set; }

    public int? ChUtId { get; set; }

    public int? ChBrId { get; set; }

    public int? ChLnId { get; set; }

    public int? ChDpId { get; set; }

    public byte[]? ChPhoto { get; set; }

    public byte[]? ChFinger1 { get; set; }

    public byte[]? ChFinger2 { get; set; }

    public int? ChPinNo { get; set; }

    public DateTime? ChValidTo { get; set; }

    public int? ChCsId { get; set; }

    public bool ChIsbio { get; set; }

    public bool ChIscard { get; set; }

    public bool ChIspin { get; set; }

    public bool ChAntiPassBack { get; set; }

    public bool ChCardIssued { get; set; }

    public bool? ChTrackCard { get; set; }

    public int? ChMsId { get; set; }

    public DateTime? ChDoj { get; set; }

    public int? ChEsId { get; set; }

    public DateTime? ChDos { get; set; }

    public DateTime ChCreated { get; set; }

    public DateTime ChModified { get; set; }

    public string ChModifiedby { get; set; } = null!;
}

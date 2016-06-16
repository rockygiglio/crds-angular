using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IFormSubmissionRepository
    {
        List<MpFormField> GetFieldsForForm(int formId);
        int GetFormFieldId(int crossroadsId);
        List<MpTripFormResponse> GetTripFormResponsesByRecordId(int recordId);
        List<MpTripFormResponse> GetTripFormResponsesBySelectionId(int selectionId);
        int SubmitFormResponse(MpFormResponse form);

        DateTime? GetTripFormResponseByContactId(int p, int pledgeId);
    }
}
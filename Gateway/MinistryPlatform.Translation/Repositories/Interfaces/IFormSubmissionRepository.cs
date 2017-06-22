using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IFormSubmissionRepository
    {
        List<MpFormField> GetFieldsForForm(int formId);
        int GetFormFieldId(int crossroadsId);
        List<MpTripFormResponse> GetTripFormResponsesByRecordId(int recordId);
        List<MpTripFormResponse> GetTripFormResponsesBySelectionId(int selectionId);
        int SubmitFormResponse(MpFormResponse form);

        DateTime? GetTripFormResponseByContactId(int p, int pledgeId);

        string GetFormResponseAnswer(int formId, int contactId, int formFieldId, int? eventId = null);
        MpFormResponse GetFormResponse(int formId, int contactId, int? eventId = null);
    }
}
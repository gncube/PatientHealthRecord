using PatientHealthRecord.UseCases.Patients;

namespace PatientHealthRecord.Web.Patients;

/// <summary>
/// Response for getting family members
/// </summary>
public class GetFamilyMembersResponse(List<PatientSummaryDto> familyMembers)
{
    public List<PatientSummaryDto> FamilyMembers { get; set; } = familyMembers;
}

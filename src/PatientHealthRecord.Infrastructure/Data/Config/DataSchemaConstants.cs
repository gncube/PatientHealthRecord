namespace PatientHealthRecord.Infrastructure.Data.Config;

public static class DataSchemaConstants
{
  public const int DEFAULT_NAME_LENGTH = 100;
  public const int EMAIL_LENGTH = 255;
  public const int PHONE_NUMBER_LENGTH = 20;
  public const int RELATIONSHIP_LENGTH = 50;
  public const int EMERGENCY_CONTACT_NAME_LENGTH = 100;
  public const int EMERGENCY_CONTACT_PHONE_LENGTH = 20;
  public const int EMERGENCY_CONTACT_RELATIONSHIP_LENGTH = 50;
  public const int BLOOD_TYPE_LENGTH = 10;
  public const int MEDICAL_NOTES_LENGTH = 2000;
  public const int ALLERGY_ITEM_LENGTH = 100;
  public const int RESTRICTED_DATA_TYPE_LENGTH = 50;

  // Clinical Data Constants
  public const int OBSERVATION_TYPE_LENGTH = 100;
  public const int OBSERVATION_VALUE_LENGTH = 500;
  public const int OBSERVATION_UNIT_LENGTH = 50;
  public const int OBSERVATION_RECORDED_BY_LENGTH = 100;
  public const int OBSERVATION_NOTES_LENGTH = 1000;

  public const int CONDITION_NAME_LENGTH = 200;
  public const int CONDITION_DESCRIPTION_LENGTH = 1000;
  public const int CONDITION_TREATMENT_LENGTH = 1000;

  public const int MEDICATION_NAME_LENGTH = 200;
  public const int MEDICATION_DOSAGE_LENGTH = 100;
  public const int MEDICATION_FREQUENCY_LENGTH = 100;
  public const int MEDICATION_INSTRUCTIONS_LENGTH = 1000;
  public const int MEDICATION_PRESCRIBED_BY_LENGTH = 100;
  public const int MEDICATION_PURPOSE_LENGTH = 500;
  public const int MEDICATION_SIDE_EFFECTS_LENGTH = 1000;
  public const int MEDICATION_RECORDED_BY_LENGTH = 100;
}

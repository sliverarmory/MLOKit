rule MLOKit_Signatures
{
    meta:
        description = "Static signatures for the MLOKit tool."
        md5 = "e977ac02118a3cb2c584d92a324e41e9"
        rev = 1
        author = "Brett Hawkins"
    strings:
        $typelibguid = "32D508EE-ADFF-4553-A5E6-300E8DF64434" ascii nocase wide
    condition:
        uint16(0) == 0x5A4D and $typelibguid
}

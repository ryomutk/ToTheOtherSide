public enum StepState
{
    undefined,
    unfound,
    found,
    revealed,
    traveled,  //これはSession中でのステートなので保存されるStateには入らない
    unavalable //現状まだ入れない
}
public class Less_Int : Operator<int?,bool?> {

    public override void CheckOutput()
    {
        if (input[0] != null && input[1] != null)
        {
            output[0] = input[0] < input[1];
            outputModules[0].Input(output[0]);
        }
    }
}

public class Less_Float : Operator<float?,bool?> {
    public override void CheckOutput()
    {
        if (input[0] != null && input[1] != null)
        {
            output[0] = input[0].Value < input[1].Value;
            outputModules[0].Input(output[0]);
        }
    }
}
